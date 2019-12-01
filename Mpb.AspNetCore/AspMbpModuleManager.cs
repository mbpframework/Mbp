using Mbp.Core.Modularity;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.AspNetCore
{
    public class AspMbpModuleManager : MbpModuleManage, IAspNetCoreUseModule
    {
        public void UseModule(IApplicationBuilder app)
        {
            foreach (MbpModule module in LoadedModules)
            {
                if (module is AspNetCoreModule aspNetCoreModule)
                {
                    aspNetCoreModule.UseModule(app);
                }
                else
                {
                    module.UseModule(app.ApplicationServices);
                }
            }
        }
    }
}
