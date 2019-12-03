using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Authentication.JwtBearer
{
    public class Jwt
    {
        /// <summary>
        /// 获取或设置 用于业务身份认证的AccessToken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 获取或设置 用于刷新AccessToken
        /// </summary>
        public string RefreshToken { get; set; }
    }
}
