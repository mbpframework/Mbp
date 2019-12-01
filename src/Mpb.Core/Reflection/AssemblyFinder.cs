using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Mbp.Core.Reflection
{
    public class AssemblyFinder : IAssemblyFinder
    {
        private readonly Lazy<IReadOnlyList<Assembly>> _assemblies;

        public IReadOnlyList<Assembly> Assemblies => _assemblies.Value;

        public AssemblyFinder()
        {
            _assemblies = new Lazy<IReadOnlyList<Assembly>>(FindAll, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public IReadOnlyList<Assembly> FindAll()
        {
            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", SearchOption.TopDirectoryOnly)
                .ToArray();

            IReadOnlyList<Assembly> assemblies = files.Select(Assembly.LoadFrom).Distinct().ToList();

            return assemblies;
        }
    }
}
