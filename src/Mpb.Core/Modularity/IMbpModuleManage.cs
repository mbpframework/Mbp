using Autofac;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Modularity
{
    public interface IMbpModuleManage
    {
        void RegisterModules(IServiceCollection services);

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        void UseModule(IServiceProvider provider);
    }
}
