using Autofac;
using Mbp.Core.Modularity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Ddd.Application
{
    public class MbpDddApplicationModule : MbpModule
    {
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            return base.AddServices(services);
        }

        public override EnumModuleGrade Level => base.Level;

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }

        public override void UseModule(IServiceProvider provider)
        {
            base.UseModule(provider);
        }
    }
}
