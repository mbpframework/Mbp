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

            // 得到配置
            var dbType = configuration.GetSection("MbpFramework:Database:DbType").Value;
            var version = configuration.GetSection("MbpFramework:Database:Version").Value;

            // 设置在框架中设置连接字符串,并根据数据库版本特性,指定分页方式
            services.AddDbContext<DefaultDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MbdbDatabase"),
                    options =>
                    {
                        if (int.Parse(version) <= 2008)
                        {
                            options.UseRowNumberForPaging();
                        }
                    });
            });

            return base.AddServices(services);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }
    }
}
