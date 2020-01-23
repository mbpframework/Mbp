using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.EntityFrameworkCore.ProviderStategy
{
    public static class DbProviderExtension
    {
        public static DbContextOptionsBuilder UseDb(this DbContextOptionsBuilder dbContextOptionsBuilder, IConfiguration configuration)
        {
            var dbType = configuration.GetSection("MbpFramework:Database:DbType").Value;
            return new ProviderRoute(dbType).UseDb(dbContextOptionsBuilder, configuration);
        }
    }
}
