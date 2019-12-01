using Autofac;
using Mbp.Core.Modularity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Mbp.EntityFrameworkCore
{
    public class MbpEntityFrameworkCoreModule : MbpModule
    {
        public override EnumModuleGrade Level => EnumModuleGrade.Component;

        protected override void Load(ContainerBuilder builder)
        {


            base.Load(builder);
        }

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            return base.AddServices(services);
        }
    }
}
