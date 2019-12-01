using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Core.Config
{
    /// <summary>
    /// Mbp框架配置文件
    /// </summary>
    public class MbpConfig
    {
        public JwtConfig Jwt { get; set; }

        public SwaggerConfig Swagger { get; set; }
    }
}
