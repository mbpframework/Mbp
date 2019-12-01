using Mbp.Core.Modularity;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Mbp.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// 使用医学大数据AspNetCore中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMedicalFramework(this IApplicationBuilder app)
        {
            IServiceProvider provider = app.ApplicationServices;

            if (!(provider.GetService<IMbpModuleManage>() is IAspNetCoreUseModule aspModuleManager))
            {
                throw new Exception("接口 IMbpModuleManage 的注入类型不正确，该类型应同时实现接口 IAspNetCoreUseModule");
            }

            aspModuleManager.UseModule(app);

            return app;
        }
    }
}
