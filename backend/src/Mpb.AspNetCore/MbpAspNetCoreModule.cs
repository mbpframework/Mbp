using Autofac;
using Mbp.AspNetCore.Mvc.Convention;
using Mbp.AspNetCore.Mvc.Filter;
using Mbp.AspNetCore.Mvc.Middleware;
using Mbp.Core.Core;
using Mbp.Core.Core.Config;
using Mbp.Core.Modularity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Mbp.AspNetCore
{
    public class MbpAspNetCoreModule : AspNetCoreModule
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }

        public override EnumModuleGrade Level => EnumModuleGrade.Component;

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddMvc().AddNewtonsoftJson(options =>
            {
                // 忽略循环引用
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                // 不使用驼峰
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // 设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }).AddMvcOptions(options =>
            {
                // 禁用Version的绑定
                options.ModelMetadataDetailsProviders.Add(new ExcludeBindingMetadataProvider(typeof(System.Version)));

                // 统一事务处理中间件
                options.Filters.Add(typeof(MbpTransActionFilter));

                // 统一日志处理中间件
                options.Filters.Add(typeof(MbpLogFilter));

                // 请求响应统一格式处理中间件
                options.Filters.Add(typeof(ResponseMiddleware));

            }); ;

            AddAutoWebApi(services, new AutoWebApiOptions());

            // 创建Cors策略
            services.AddCors(options =>
            {
                options.AddPolicy("MbpCors",
                builder =>
                {
                    builder.WithOrigins(services.BuildServiceProvider().GetService<IConfiguration>().GetSection("AllowedHosts").Value)
                    .AllowAnyMethod()
                    .AllowAnyHeader(); ;
                });
            });

            return base.AddServices(services);
        }

        private IServiceCollection AddAutoWebApi(IServiceCollection services, AutoWebApiOptions options)
        {
            if (options == null)
            {
                throw new ArgumentException(nameof(options));
            }

            options.Valid();

            AppConsts.DefaultAreaName = options.DefaultAreaName;
            AppConsts.DefaultHttpVerb = options.DefaultHttpVerb;
            AppConsts.DefaultApiPreFix = options.DefaultApiPrefix;
            AppConsts.ControllerPostfixes = options.RemoveControllerPostfixes;
            AppConsts.ActionPostfixes = options.RemoveActionPostfixes;
            AppConsts.FormBodyBindingIgnoredTypes = options.FormBodyBindingIgnoredTypes;

            var partManager = services.GetSingletonInstanceOrNull<ApplicationPartManager>();

            if (partManager == null)
            {
                throw new InvalidOperationException("\"AddAutoWebApi\" must be after \"AddMvc\".");
            }

            // Add a custom controller checker
            partManager.FeatureProviders.Add(new MbpConventionalControllerFeatureProvider());

            services.Configure<MvcOptions>(o =>
            {
                // Register Controller Routing Information Converter
                o.Conventions.Add(new MbpServiceConvention(services));
            });

            return services;
        }

        public override void UseModule(IApplicationBuilder app)
        {
            // 启用跨域请求中间件
            app.UseCors("MbpCors");

            // 启用应用服务层全局错误处理中间件
            app.UseMiddleware(typeof(MbpGlobaExceptionMiddleware));

            base.UseModule(app);
        }
    }
}
