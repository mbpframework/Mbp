using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Medical.Ai.Mbdp.EntityFrameworkCore.ProviderStategy
{
    public class SqlServerProvider : IDbProviderStategy
    {
        public DbContextOptionsBuilder UseDb(DbContextOptionsBuilder dbContextOptionsBuilder, IConfiguration configuration)
        {
            var dbType = configuration.GetSection("MbpFramework:Database:DbType").Value;
            var version = configuration.GetSection("MbpFramework:Database:Version").Value;

            return dbContextOptionsBuilder.UseSqlServer(configuration.GetConnectionString("MbdbDatabase"),
                        o =>
                        {
                            if (int.Parse(version) <= 2008)
                            {
                                o.UseRowNumberForPaging();
                            }
                        });
        }
    }
}
