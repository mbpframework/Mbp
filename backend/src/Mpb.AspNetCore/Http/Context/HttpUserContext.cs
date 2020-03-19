using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.AspNetCore.Http.Context
{
    /// <summary>
    /// 用户信息,写入到token里面,每次回传到服务器验证通过后,将为每个请求创建一个用户实例,生命周期:作用域生存期服务 (AddScoped) 以每个客户端请求（连接）一次的方式创建
    /// </summary>
    public class HttpUserContext
    {
        /// <summary>
        ///  用户编码
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 租户名称
        /// </summary>
        public int TenantName { get; set; }

    }
}
