using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Mbp.Core.Reflection;
using Mbp.Core.Core;

namespace Mbp.AspNetCore.Mvc.Convention
{
    public class MbpConventionalControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            var type = typeInfo.AsType();

            if (!typeof(IAppService).IsAssignableFrom(type) ||
                !typeInfo.IsPublic || typeInfo.IsAbstract || typeInfo.IsGenericType)
            {
                return false;
            }

            var attr = ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<AutoWebApiAttribute>(typeInfo);

            if (attr == null)
            {
                return false;
            }

            if (ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<NoneWebApiAttribute>(typeInfo) != null)
            {
                return false;
            }

            return true;
        }
    }
}
