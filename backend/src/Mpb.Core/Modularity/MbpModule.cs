using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Modularity
{
    public abstract class MbpModule : Module, IMbpModule
    {
        /// <summary>
        /// 模块级别,为了安全,其他模块不定义Core, TO DO 将设计成模块依赖 来决定加载顺序
        /// </summary>
        public virtual EnumModuleGrade Level => EnumModuleGrade.Application;

        /// <summary>
        /// 支持autofac依赖注入方式
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
        }

        /// <summary>
        /// 支持.net core自带的依赖注入方式
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public virtual IServiceCollection AddServices(IServiceCollection services)
        {
            return services;
        }

        /// <summary>
        /// 使用模块,主要应用于AspNetCore环境的
        /// </summary>
        /// <param name="provider"></param>
        public virtual void UseModule(IServiceProvider provider)
        {

        }
    }
}
