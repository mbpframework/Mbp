using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Authentication
{
    /// <summary>
    /// 模块授权点配置和存储的处理类
    /// 应用程序启动的时候,将模块授权点在存储器中暂存,模块授权点随应用程序的退出而在内存中销毁
    /// </summary>
    public class PermissionPolicy
    {
        private readonly PermissionClaim _permissionClaim = null;

        public PermissionPolicy(PermissionClaim permissionClaim)
        {
            _permissionClaim = permissionClaim;

            // 将模块权限点写到存储器中
            PermissionCacheStorage.Instance.SetValue(permissionClaim.PermissionGroupName, permissionClaim);
        }
    }
}
