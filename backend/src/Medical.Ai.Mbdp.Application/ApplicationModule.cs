using Autofac;
using Autofac.Extras.DynamicProxy;
using Mbp.Authentication;
using Mbp.Core.Aop;
using Mbp.Core.Core;
using Mbp.Core.Core.Dependency;
using Mbp.Core.Modularity;
using Medical.Ai.Mbdp.Application.Contracts;
using Medical.Ai.Mbdp.Application.Demo;
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
using Medical.Ai.Mbdp.Application.AuthenticatePolicyHanlder;

namespace Medical.Ai.Mbdp.Application
{
    public class ApplicationModule : MbpModule
    {
        public override EnumModuleGrade Level => EnumModuleGrade.Application;
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // 注册OOM
            builder.RegisterInstance(AutoMapperConfig.CreateMapper()).SingleInstance();
        }

        public override IServiceCollection AddServices(IServiceCollection services)
        {
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
