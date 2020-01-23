using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.EntityFrameworkCore.ProviderStategy
{
    public class ProviderRoute
    {
        private IDbProviderStategy _dbProviderStategy = null;

        public ProviderRoute(string DbType)
        {
            switch (DbType)
            {
                case "SQL Server":
                    _dbProviderStategy = new SqlServerProvider();
                    break;
                case "MySql":
                    _dbProviderStategy = new MysqlProvider();
                    break;
                default:
                    _dbProviderStategy = new SqlServerProvider();
                    break;
            }
        }

        public DbContextOptionsBuilder UseDb(DbContextOptionsBuilder dbContextOptionsBuilder, IConfiguration configuration)
        {
            return _dbProviderStategy.UseDb(dbContextOptionsBuilder, configuration);
        }
    }
}
