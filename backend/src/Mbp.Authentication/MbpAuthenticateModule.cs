using Autofac;
using Mbp.Core.Modularity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mbp.Authentication
{
    public class MbpAuthenticateModule : AspNetCoreModule
    {
        public override EnumModuleGrade Level => EnumModuleGrade.Component;

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            // 添加全局授权策略
            services.AddAuthorization(options =>
            {
                options.AddPolicy("GlobalPermission", policy => policy.Requirements.Add(new PermissionRequirement()));
            });

            return base.AddServices(services);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }

        public override void UseModule(IApplicationBuilder app)
        {
            base.UseModule(app);
        }
    }
}
