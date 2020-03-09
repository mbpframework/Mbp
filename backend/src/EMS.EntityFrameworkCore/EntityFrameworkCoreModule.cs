using Autofac;
using Mbp.EntityFrameworkCore;
using EMS.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using EMS.EntityFrameworkCore.ProviderStategy;

namespace EMS.EntityFrameworkCore
{
    public class EntityFrameworkCoreModule : MbpEntityFrameworkCoreModule
    {
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            // 获取主机配置对象
            IConfiguration configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            // 设置数据库连接
            services.AddDbContext<DefaultDbContext>(options =>
            {
                options.UseDb(configuration);
            });

            return base.AddServices(services);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}
