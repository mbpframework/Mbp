using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Mbp.Core.Core.System;
using Mbp.Core.Reflection;
using Mbp.Core.Core;


namespace Mbp.AspNetCore.Mvc.Convention
{
    public class MbpServiceConvention : IApplicationModelConvention
    {
        private readonly IServiceCollection _services;
        public MbpServiceConvention(IServiceCollection services)
        {
            this._services = services;
        }

        /// <summary>
        /// 定制应用模型
        /// </summary>
        /// <param name="application"></param>
        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var type = controller.ControllerType.AsType();

                var dynamicWebApiAttr = ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<AutoWebApiAttribute>(type.GetTypeInfo());

                // 自定义的应用服务
                if (typeof(IAppService).GetTypeInfo().IsAssignableFrom(type))
                {
                    controller.ControllerName = controller.ControllerName.RemovePostFix(AppConsts.ControllerPostfixes.ToArray());
                    ConfigureApplicationService(controller, dynamicWebApiAttr);
                }
                else
                {
                    // Web Api控制器
                    if (dynamicWebApiAttr != null)
                    {
                        ConfigureApplicationService(controller, dynamicWebApiAttr);
                    }
                }
            }
        }

        protected void ConfigureApplicationService(ControllerModel controller, AutoWebApiAttribute controllerAttr)
        {
            ConfigureApiExplorer(controller);
            ConfigureSelector(controller, controllerAttr);
            ConfigureParameters(controller);
        }

        protected void ConfigureParameters(ControllerModel controller)
        {
            foreach (var action in controller.Actions)
            {
                foreach (var para in action.Parameters)
                {
                    if (para.BindingInfo != null)
                    {
                        continue;
                    }

                    if (!TypeHelper.IsPrimitiveExtendedIncludingNullable(para.ParameterInfo.ParameterType))
                    {
                        if (CanUseFormBodyBinding(action, para))
                        {
                            para.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                        }
                    }
                }
            }
        }

        protected bool CanUseFormBodyBinding(ActionModel action, ParameterModel parameter)
        {
            if (AppConsts.FormBodyBindingIgnoredTypes.Any(t => t.IsAssignableFrom(parameter.ParameterInfo.ParameterType)))
            {
                return false;
            }

            foreach (var selector in action.Selectors)
            {
                if (selector.ActionConstraints == null)
                {
                    continue;
                }

                foreach (var actionConstraint in selector.ActionConstraints)
                {
                    var httpMethodActionConstraint = actionConstraint as HttpMethodActionConstraint;
                    if (httpMethodActionConstraint == null)
                    {
                        continue;
                    }

                    if (httpMethodActionConstraint.HttpMethods.All(hm => hm.IsIn("GET", "DELETE", "TRACE", "HEAD")))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        protected void ConfigureApiExplorer(ControllerModel controller)
        {
            if (controller.ApiExplorer.GroupName.IsNullOrEmpty())
            {
                controller.ApiExplorer.GroupName = controller.ControllerName;
            }

            if (controller.ApiExplorer.IsVisible == null)
            {
                controller.ApiExplorer.IsVisible = true;
            }

            foreach (var action in controller.Actions)
            {
                ConfigureApiExplorer(action);
            }
        }

        protected void ConfigureApiExplorer(ActionModel action)
        {
            if (action.ApiExplorer.IsVisible == null)
            {
                action.ApiExplorer.IsVisible = true;
            }
        }

        protected void ConfigureSelector(ControllerModel controller, AutoWebApiAttribute controllerAttr)
        {
            RemoveEmptySelectors(controller.Selectors);

            if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
            {
                return;
            }

            var rootPath = string.Empty;

            if (controllerAttr != null)
            {
                rootPath = controllerAttr.Module;
            }

            foreach (var action in controller.Actions)
            {
                ConfigureSelector(rootPath, controller.ControllerName, action);
            }
        }

        protected void ConfigureSelector(string rootPath, string controllerName, ActionModel action)
        {
            RemoveEmptySelectors(action.Selectors);

            var nonAttr = ReflectionHelper.GetSingleAttributeOrDefault<NoneWebApiAttribute>(action.ActionMethod);

            if (nonAttr != null)
            {
                return;
            }

            if (!action.Selectors.Any())
            {
                AddAppServiceSelector(rootPath, controllerName, action);
            }
            else
            {
                NormalizeSelectorRoutes(rootPath, controllerName, action);
            }
        }

        protected void AddAppServiceSelector(string rootPath, string controllerName, ActionModel action)
        {
            string verb = GetConventionalVerbForMethodName(action);

            var appServiceSelectorModel = new SelectorModel
            {
                AttributeRouteModel = CreateActionRouteModel(rootPath, controllerName, action)
            };

            appServiceSelectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { verb }));

            action.Selectors.Add(appServiceSelectorModel);
        }

        protected string GetRestFulActionName(string actionName)
        {
            // 如果不启用Restful风格
            //if (!bool.Parse(_services.BuildServiceProvider().GetService<IConfiguration>().GetSection("IsRestful").Value))
            return actionName;


            // Remove Postfix
            actionName = actionName.RemovePostFix(AppConsts.ActionPostfixes.ToArray());

            // Remove Prefix
            var verbKey = actionName.GetPascalOrCamelCaseFirstWord().ToLower();
            if (AppConsts.HttpVerbs.ContainsKey(verbKey))
            {
                if (actionName.Length == verbKey.Length)
                {
                    return "";
                }
                else
                {
                    return actionName.Substring(verbKey.Length);
                }
            }
            else
            {
                return actionName;
            }
        }

        protected void NormalizeSelectorRoutes(string rootPath, string controllerName, ActionModel action)
        {
            foreach (var selector in action.Selectors)
            {
                // 防止正常的控制器类没有指定 HttpMethod，根据约定加上
                string verb = GetConventionalVerbForMethodName(action);

                if (!selector.ActionConstraints.OfType<HttpMethodActionConstraint>().Any())
                {
                    selector.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { verb }));
                }

                // 没有指定路由会同Nitrogen的应用服务一起被定制 否则会使用指定的路由
                selector.AttributeRouteModel = selector.AttributeRouteModel == null ?
                    CreateActionRouteModel(rootPath, controllerName, action) :
                    AttributeRouteModel.CombineAttributeRouteModel(CreateActionRouteModel(rootPath, controllerName), selector.AttributeRouteModel);

            }
        }

        private static string GetConventionalVerbForMethodName(ActionModel action)
        {
            string verb;
            var verbKey = action.ActionName.GetPascalOrCamelCaseFirstWord().ToLower();
            verb = AppConsts.HttpVerbs.ContainsKey(verbKey) ? AppConsts.HttpVerbs[verbKey] : AppConsts.DefaultHttpVerb;
            return verb;
        }

        protected AttributeRouteModel CreateActionRouteModel(string rootPath, string controllerName)
        {
            var routeStr =
               $"{AppConsts.DefaultApiPreFix}/{rootPath}/{controllerName}".Replace("//", "/");

            return new AttributeRouteModel(new RouteAttribute(routeStr));
        }

        protected AttributeRouteModel CreateActionRouteModel(string rootPath, string controllerName, ActionModel action)
        {
            action.ActionName = GetRestFulActionName(action.ActionName);

            var routeStr =
                $"{AppConsts.DefaultApiPreFix}/{rootPath}/{controllerName}/{action.ActionName}".Replace("//", "/");

            var idParameterModel = action.Parameters.FirstOrDefault(p => p.ParameterName == "id");
            if (idParameterModel != null)
            {
                if (TypeHelper.IsPrimitiveExtended(idParameterModel.ParameterType, includeEnums: true))
                {
                    routeStr += "/{id}";
                }
                else
                {
                    var properties = idParameterModel
                        .ParameterType
                        .GetProperties(BindingFlags.Instance | BindingFlags.Public);

                    foreach (var property in properties)
                    {
                        routeStr += "/{" + property.Name + "}";
                    }
                }
            }

            return new AttributeRouteModel(new RouteAttribute(routeStr));
        }

        protected void RemoveEmptySelectors(IList<SelectorModel> selectors)
        {
            selectors
                .Where(IsEmptySelector)
                .ToList()
                .ForEach(s => selectors.Remove(s));
        }

        protected bool IsEmptySelector(SelectorModel selector)
        {
            return selector.AttributeRouteModel == null
                   && selector.ActionConstraints.IsNullOrEmpty()
                   // 防止Authorize被移除
                   && selector.EndpointMetadata.IsNullOrEmpty();
        }
    }
}
