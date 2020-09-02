using Autofac;
using Autofac.Extras.DynamicProxy;
using Mbp.Authentication;
using Mbp.Core.Aop;
using Mbp.Core.Core;
using Mbp.Core.Core.Dependency;
using Mbp.Core.Modularity;
using EMS.Application.Contracts;
using EMS.Application.Demo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using EMS.Application.AuthenticatePolicyHanlder;
using Mbp.Ddd.Application;

namespace EMS.Application
{
    public class ApplicationModule : MbpDddApplicationModule
    {
        public override EnumModuleGrade Level => EnumModuleGrade.Application;
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //builder.RegisterType(typeof(LogInterceptor));

            // 所有集成IDomainService的类型都会被扫入IOC
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
            //          .Where(t => t.Name.EndsWith("AppService")
            //          && typeof(IAppService).IsAssignableFrom(t)
            //          && typeof(IPerRequestDependency).IsAssignableFrom(t)
            //          )
            //          .AsImplementedInterfaces().InstancePerDependency()
            //          .EnableClassInterceptors()
            //          .InterceptedBy(typeof(LogInterceptor));

           

            


            // 注册OOM
            builder.RegisterInstance(AutoMapperConfig.CreateMapper()).SingleInstance();
        }

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            //services.AddControllers().AddControllersAsServices();

            // 注册全局授权策略处理程序
            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();

            // 配置权限点,todo 决定是否需要自动配置
            PermissionClaim permissionClaim = new PermissionClaim()
            {
                PermissionGroupName = "Demo",
                Claims = new List<Claim>()
                {
                    new Claim("GetBlogs","GetBlogs"),
                    new Claim("DeleteBlog", "DeleteBlog"),
                    new Claim("AddBlog", "AddBlog")
                }
            };

            return base.AddServices(services);
        }
    }
}
