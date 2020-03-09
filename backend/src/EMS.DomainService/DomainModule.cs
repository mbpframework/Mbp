using Autofac;
using Mbp.Core.Core;
using Mbp.Core.Core.Dependency;
using Mbp.Core.Modularity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EMS.Domain
{
    public class DomainModule : MbpModule
    {
        public override EnumModuleGrade Level => base.Level;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            // 所有集成IDomainService的类型都会被扫入IOC
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                      .Where(t => t.Name.EndsWith("DomainService")
                      && typeof(IDomainService).IsAssignableFrom(t)
                      && typeof(IPerRequestDependency).IsAssignableFrom(t)
                      )
                      .AsImplementedInterfaces().InstancePerDependency();
        }

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            return base.AddServices(services);
        }
    }
}
