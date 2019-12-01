using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Mbp.Core.Modularity
{
    /// <summary>
    /// 
    /// </summary>
    public static class AutofacService
    {
        private static readonly ContainerBuilder Builder = new ContainerBuilder();

        private static IContainer _container;

        private static readonly List<Module> Modules;

        private static String[] _assmblies;

        private static readonly List<Type> Types;

        private static readonly Dictionary<Type, Type> DictionaryTypes;

        static AutofacService()
        {
            Modules = new List<Module>();
            Types = new List<Type>();
            DictionaryTypes = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// 注册程序集
        /// </summary>
        /// <param name="assembies"></param>
        public static void Register(params string[] assembies)
        {
            _assmblies = assembies;
        }

        /// <summary>
        /// 注册程序集。
        /// </summary>
        /// <param name="implementationAssemblyName"></param>
        /// <param name="interfaceAssemblyName"></param>
        public static void Register(string implementationAssemblyName, string interfaceAssemblyName)
        {
            var implementationAssembly = System.Reflection.Assembly.Load(implementationAssemblyName);
            var interfaceAssembly = System.Reflection.Assembly.Load(interfaceAssemblyName);
            var implementationTypes =
                implementationAssembly.DefinedTypes.Where(t =>
                    t.IsClass && !t.IsAbstract && !t.IsGenericType && !t.IsNested);
            foreach (var type in implementationTypes)
            {
                var interfaceTypeName = interfaceAssemblyName + ".I" + type.Name;
                var interfaceType = interfaceAssembly.GetType(interfaceTypeName);
                if (interfaceType.IsAssignableFrom(type))
                {
                    DictionaryTypes.Add(interfaceType, type);
                }
            }
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="module"></param>
        public static void RegisterModule(Module module)
        {
            Modules.Add(module);
        }

        /// <summary>
        /// 注册类型
        /// </summary>
        /// <param name="types"></param>
        public static void Register(params Type[] types)
        {
            Types.AddRange(types.ToList());
        }

        /// <summary>
        /// 注册实例化对象和接口
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        public static void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            DictionaryTypes.Add(typeof(TInterface), typeof(TImplementation));
        }

        /// <summary>
        /// 注册对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void Register<T>(T instance) where T : class
        {
            Builder.RegisterInstance(instance).SingleInstance();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Resolve<T>()
        {
            using (var scope = _container.BeginLifetimeScope())
            {
                return scope.Resolve<T>();
            }
        }

        /// <summary>
        /// 构建IOC容器，需在各种Register后调用。
        /// </summary>
        public static void Build()
        {
            if (_assmblies != null)
            {
                foreach (var item in _assmblies)
                {
                    Builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load(item));
                }
            }

            if (Types != null)
            {
                foreach (var type in Types)
                {
                    Builder.RegisterType(type);
                }
            }

            if (DictionaryTypes != null)
            {
                foreach (var dicType in DictionaryTypes)
                {
                    Builder.RegisterType(dicType.Value).As(dicType.Key);
                }
            }

            if (Modules != null)
            {
                foreach (var item in Modules)
                {
                    Builder.RegisterModule(item);
                }
            }

            _container = Builder.Build();
        }
    }
}
