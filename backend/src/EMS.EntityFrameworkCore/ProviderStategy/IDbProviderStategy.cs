using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.EntityFrameworkCore.ProviderStategy
{
    /// <summary>
    /// 切换不同数据库类型的策略
    /// </summary>
    public interface IDbProviderStategy
    {
        // 数据库类型和版本
        DbContextOptionsBuilder UseDb(DbContextOptionsBuilder dbContextOptionsBuilder, IConfiguration configuration);

    }
}
