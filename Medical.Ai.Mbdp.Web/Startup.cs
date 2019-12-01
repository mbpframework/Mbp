using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mbp.Core.Core;
using Mbp.AspNetCore;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Mbp.Authentication;

namespace Medical.Ai.Mbdp.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // DI---->IOC
        public void ConfigureServices(IServiceCollection services)
        {
            // 加载医学大数据平台框架
            services.AddMedicalFramework<AspMbpModuleManager>();
        }

        // asp.net core 管道
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 使用医学大数据开发平台框架
            app.UseMedicalFramework();

            // 路由中间件
            app.UseRouting();

            // 鉴权中间件
            app.UseAuthorization();

            // 路由终结点配置 开启终结点之后,mbp的权限过滤器将以中间件的形式独立运行,不会再添加到ActionDescriptor 
            // 也就是说,我们不要选择以这种方式来拦截和自定义鉴权算法
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
