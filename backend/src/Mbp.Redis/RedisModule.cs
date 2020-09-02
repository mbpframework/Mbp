using Autofac;
using Mbp.Core.Core.Config;
using Mbp.Core.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Redis
{
    public class RedisModule : AspNetCoreModule
    {
        public override EnumModuleGrade Level => EnumModuleGrade.Component;

        public override void UseModule(IApplicationBuilder app)
        {
            base.UseModule(app);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            // 获取配置
            var serviceProvider = services.BuildServiceProvider();
            var redisConfig = serviceProvider.GetService<IOptions<MbpConfig>>().Value?.Redis;

            // 注册缓存连接对象
            ConfigurationOptions configuration = new ConfigurationOptions();
            redisConfig.EndPoints.ForEach(hostAndPort =>
            {
                configuration.EndPoints.Add(hostAndPort);
            });
            configuration.CommandMap = CommandMap.Create(new HashSet<string>
                { // EXCLUDE a few commands
                    "INFO", "CONFIG", "CLUSTER",
                    "PING", "ECHO", "CLIENT"
                }, available: false);
            configuration.KeepAlive = redisConfig.KeepAlive;
            configuration.DefaultVersion = new Version(redisConfig.DefaultVersion);
            configuration.Password = redisConfig.Password;

            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(configuration);
            services.AddSingleton<ConnectionMultiplexer>(redis);

            // 注册缓存服务
            services.TryAddScoped<IRedisService, RedisService>();

            return base.AddServices(services);
        }
    }
}
