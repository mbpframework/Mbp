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
using Autofac;
using Mbp.Core.Aop;
using Autofac.Extras.DynamicProxy;
using EMS.Application.Demo;

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
            // ����mbpƽ̨���
            services.AddMedicalFramework<AspMbpModuleManager>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(LogInterceptor));

            builder.RegisterType<DemoAppService>().EnableClassInterceptors().InterceptedBy(typeof(LogInterceptor)); 

            builder.RegisterType<ValuesController>().EnableClassInterceptors().InterceptedBy(typeof(LogInterceptor));
        }

        // asp.net core �ܵ�,�ܵ�˳���΢�������˵����Щ��ͻ,������Դ�����м����Ҫ��Routing��EndPoints֮��,������ֻ������EndPoints֮ǰ
        // �������������ھ���,��ʱ�����������м��˳��.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ʹ��mbp����ƽ̨���
            app.UseMedicalFramework();

            // ·���м��
            app.UseRouting();

            // �����֤�м��
            app.UseAuthorization();

            // ����ҵ���Ʒ�ļ�����·��  todo��װ�������
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "FileRoot")),
                RequestPath = "/files"
            });

            // ·���ս������ �����ս��֮��,mbp��Ȩ�޹����������м������ʽ��������,��������ӵ�ActionDescriptor 
            // Ҳ����˵,���ǲ�Ҫѡ�������ַ�ʽ�����غ��Զ����Ȩ�㷨
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
