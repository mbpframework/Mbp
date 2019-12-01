using Autofac;
using Mbp.EntityFrameworkCore;
using Medical.Ai.Mbdp.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Medical.Ai.Mbdp.EntityFrameworkCore
{
    public class EntityFrameworkCoreModule : MbpEntityFrameworkCoreModule
    {
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddDbContext<DefaultDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("MbdbDatabase")));

            return base.AddServices(services);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}
