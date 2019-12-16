using LogDashboard;
using Mbp.Core.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.LogDashboard
{
    public class MbpLogDashboardModule : AspNetCoreModule
    {
        public override EnumModuleGrade Level => EnumModuleGrade.Component;

        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.AddLogDashboard();
            return base.AddServices(services);
        }

        public override void UseModule(IApplicationBuilder app)
        {
            app.UseLogDashboard();
            base.UseModule(app);
        }
    }
}
