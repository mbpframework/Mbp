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
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace EMS.Web
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
            // 加载mbp平台框架
            services.AddMedicalFramework<AspMbpModuleManager>();
        }

        // asp.net core 管道,管道顺序和微软官网的说明有些冲突,跨域资源共享中间件需要在Routing和EndPoints之间,而这里只满足在EndPoints之前
        // 后续发现问题在纠正,届时将调整所有中间件顺序.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 使用mbp开发平台框架
            app.UseMedicalFramework();

            // 路由中间件
            app.UseRouting();

            // 身份认证中间件
            app.UseAuthorization();

            // 配置业务产品文件放置路径  todo封装到框架中
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "FileRoot")),
                RequestPath = "/files"
            });

            // 路由终结点配置 开启终结点之后,mbp的权限过滤器将以中间件的形式独立运行,不会再添加到ActionDescriptor 
            // 也就是说,我们不要选择以这种方式来拦截和自定义鉴权算法
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
