using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Mbp.Authentication
{
    public class PermissionClaim
    {
        /// <summary>
        /// 权限分组名称,按照应用服务的类型来命名
        /// </summary>
        public string PermissionGroupName { get; set; }

        /// <summary>
        /// 权限下的声明,对应应用服务提供的action
        /// </summary>
        public List<Claim> Claims { get; set; } = new List<Claim>();
    }
}
