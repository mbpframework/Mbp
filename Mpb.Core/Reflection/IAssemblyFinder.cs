using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mbp.Core.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAssemblyFinder
    {
        IReadOnlyList<Assembly> Assemblies { get; }
    }
}
