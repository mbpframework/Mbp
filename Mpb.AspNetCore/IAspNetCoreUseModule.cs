using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.AspNetCore
{
    public interface IAspNetCoreUseModule
    {
        void UseModule(IApplicationBuilder app);
    }
}
