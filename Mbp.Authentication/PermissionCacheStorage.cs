using Mbp.Core.CacheStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Authentication
{
    /// <summary>
    /// 应用程序模块权限点存储,使用单例模式
    /// </summary>
    public class PermissionCacheStorage: CacheStorage<PermissionClaim>
    {
        public static PermissionCacheStorage Instance { get; } = new PermissionCacheStorage();

        static PermissionCacheStorage() { }

        private PermissionCacheStorage() { }
    }
}
