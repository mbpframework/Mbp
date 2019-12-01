using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mbp.Core.Modularity;
using Mbp.Core.Reflection;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Autofac;

namespace Mbp.Core.Core
{
    /// <summary>
    /// IServiceCollection扩展
    /// </summary>
    public static class ServiceExtensions
    {
        /// <summary>
        /// 使用医学大数据框架
        /// </summary>
        /// <typeparam name="TMbpPackManager"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddMedicalFramework<TMbpPackManager>(this IServiceCollection services)
               where TMbpPackManager : IMbpModuleManage, new()
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 注入程序集查找器
            services.TryAddSingleton<IAssemblyFinder>(new AssemblyFinder());

            TMbpPackManager manager = new TMbpPackManager();

            // 注册模块
            manager.RegisterModules(services);

            services.AddSingleton<IMbpModuleManage>(manager);

            return services;
        }
    }
}
