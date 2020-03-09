using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace EMS.EntityFrameworkCore.ProviderStategy
{
    public class MysqlProvider : IDbProviderStategy
    {
        public DbContextOptionsBuilder UseDb(DbContextOptionsBuilder dbContextOptionsBuilder, IConfiguration configuration)
        {
            // 得到配置
            var dbType = configuration.GetSection("MbpFramework:Database:DbType").Value;
            var version = configuration.GetSection("MbpFramework:Database:Version").Value;

            return dbContextOptionsBuilder.UseMySql(configuration.GetConnectionString("MbdbDatabase"),
                mySqlOptions =>
                {
                    mySqlOptions
                       .ServerVersion(new ServerVersion(new Version(version), ServerType.MySql));
                     });
        }
    }
}
