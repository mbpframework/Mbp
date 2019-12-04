using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Core.Config
{
    public class DatabaseConfig
    {
        //  "DbType": "SQL Server",
        //"Version": "2012"

        /// <summary>
        /// 数据库类型,默认为SQL Server,可选项可以为SQL Server,MySQL
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 数据库版本类型,默认位2012
        /// </summary>
        public string Version { get; set; }
    }
}
