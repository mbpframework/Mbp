using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.AspNetCore.Mvc.Convention
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Method)]
    public class NoneWebApiAttribute : Attribute
    {
    }
}
