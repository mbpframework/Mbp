using Autofac;
using Mbp.Core.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using IContainer = Autofac.IContainer;

namespace Mbp.Core.Modularity
{
    public class MbpModuleManage : IMbpModuleManage
    {
        private readonly List<MbpModule> _sourceModules;

        /// <summary>
        /// 
        /// </summary>
        public MbpModuleManage()
        {
            _sourceModules = new List<MbpModule>();
            LoadedModules = new List<MbpModule>();
        }

        /// <summary>
        /// 获取运行时下的所有模块
        /// </summary>
        public IEnumerable<MbpModule> SourceModules => _sourceModules;

        /// <summary>
        /// 获取最终加载的模块信息集合
        /// </summary>
        public IEnumerable<MbpModule> LoadedModules { get; private set; }

        /// <summary>
        /// 注册模块,从核心模块===>组件模块===>应用模块
        /// </summary>
        /// <param name="builder"></param>
        public void RegisterModules(IServiceCollection services)
        {
            IAssemblyFinder assemblyFinder = new AssemblyFinder();

            var assemblys = assemblyFinder.Assemblies;

            _sourceModules.Clear();

            // 指定模块抽象类型
            var type = typeof(IMbpModule);

            // 搜索所有符合的模块
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                .Where(p =>
                type.IsAssignableFrom(p) &&
                !p.IsAbstract &&
                p.Name != "IMbpModule")
                .ToArray();

            _sourceModules.AddRange(types.Select(m => (MbpModule)Activator.CreateInstance(m)));

            // 排序模块
            var modules = _sourceModules.OrderBy(m => m.Level).ToList();

            LoadedModules = modules;

            // 注册所有模块
            foreach (var module in LoadedModules)
            {
                module.AddServices(services);

                AutofacService.RegisterModule(module);
            }

            AutofacService.Build();
        }

        public void UseModule(IServiceProvider provider)
        {
            foreach (MbpModule pack in LoadedModules)
            {
                pack.UseModule(provider);
            }
        }
    }
}
