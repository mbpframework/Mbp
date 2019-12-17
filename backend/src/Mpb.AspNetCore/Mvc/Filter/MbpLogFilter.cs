using Mbp.Core.Core;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;

namespace Mbp.AspNetCore.Mvc.Filter
{
    public class MbpLogFilter : IAsyncActionFilter
    {
        private ILogger<MbpLogFilter> _logger;

        public MbpLogFilter(ILogger<MbpLogFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            var methodInfo = actionDescriptor.MethodInfo;
            var actionName = context.Controller.ToString();

            var t = context.HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            var loginName = "mbp匿名用户";
            if (t != null)
            {
                loginName = t.Value;
            }

            //必须显示在服务类上定义AutoAopAttribute特性
            if (methodInfo.DeclaringType.IsDefined(typeof(AutoAopAttribute), true)
                && ((AutoAopAttribute)methodInfo.DeclaringType.GetCustomAttributes(typeof(AutoAopAttribute), true)[0]).IsAutoLog)
            {
                //可以通过在方法上定义特性头来避开拦截 比如[AutoAop(IsAutoLog=false)]
                if ((methodInfo.IsDefined(typeof(AutoAopAttribute), true)
                    && !((AutoAopAttribute)methodInfo.GetCustomAttributes(typeof(AutoAopAttribute), false)[0]).IsAutoLog)
                    || (methodInfo.IsDefined(typeof(AutoAopAttribute), true)
                    && !((AutoAopAttribute)methodInfo.GetCustomAttributes(typeof(AutoAopAttribute), false)[0]).IsAutoLog))
                {
                    await next();
                }
                else
                {
                    var result = await next();

                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append("[");
                    int i = 1;
                    foreach (var argument in context.ActionArguments)
                    {
                        stringBuilder.Append($"参数{1}名:{argument.Key},参数{i}值:{argument.Value}@@");
                        i++;
                    }
                    stringBuilder.Append("]");

                    _logger.LogInformation($"用户:{loginName},正在执行方法:{actionName + "/" + methodInfo.Name},参数:{stringBuilder.ToString()}");
                }
            }
            else
            {
                await next();
            }
        }
    }
}
