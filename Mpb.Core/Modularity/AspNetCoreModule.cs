using Mbp.Core.Core.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Modularity
{
    /// <summary>
    /// 基于AspNetCore环境的模块基类
    /// </summary>
    public abstract class AspNetCoreModule : MbpModule
    {
        /// <summary>
        /// 应用AspNetCore的服务业务
        /// </summary>
        /// <param name="app">AspNetCore应用程序构建器</param>
        public virtual void UseModule(IApplicationBuilder app)
        {
            base.UseModule(app.ApplicationServices);
        }

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            // 获取Mbp全局配置
            services.Configure<MbpConfig>(services.BuildServiceProvider().GetService<IConfiguration>().GetSection("MbpFramework"));

            // 注入全局HttpContextAccessor
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return base.AddServices(services);
        }
    }
}
