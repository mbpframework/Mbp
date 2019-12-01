using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Core.Config
{
    public class JwtConfig
    {
        /// <summary>
        /// 密钥
        /// </summary>
        public string SecurityKey { get; set; }

        /// <summary>
        /// 发行方
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 订阅方
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 过期时间 Minutes
        /// </summary>
        public int TimeOut { get; set; }
    }
}
