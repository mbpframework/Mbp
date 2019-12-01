using Mbp.Core.Reflection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mbp.AspNetCore.Mvc.Convention
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class AutoWebApiAttribute : Attribute
    {
        public string Module { get; set; }

        internal static bool IsExplicitlyEnabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<AutoWebApiAttribute>();
            return remoteServiceAttr != null;
        }

        internal static bool IsExplicitlyDisabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<AutoWebApiAttribute>();
            return remoteServiceAttr != null;
        }

        internal static bool IsMetadataExplicitlyEnabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<AutoWebApiAttribute>();
            return remoteServiceAttr != null;
        }

        internal static bool IsMetadataExplicitlyDisabledFor(Type type)
        {
            var remoteServiceAttr = type.GetTypeInfo().GetSingleAttributeOrNull<AutoWebApiAttribute>();
            return remoteServiceAttr != null;
        }

        internal static bool IsMetadataExplicitlyDisabledFor(MethodInfo method)
        {
            var remoteServiceAttr = method.GetSingleAttributeOrNull<AutoWebApiAttribute>();
            return remoteServiceAttr != null;
        }

        internal static bool IsMetadataExplicitlyEnabledFor(MethodInfo method)
        {
            var remoteServiceAttr = method.GetSingleAttributeOrNull<AutoWebApiAttribute>();
            return remoteServiceAttr != null;
        }
    }
}
