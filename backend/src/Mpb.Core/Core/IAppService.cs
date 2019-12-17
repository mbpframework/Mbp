using Mbp.Core.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Core
{
    /// <summary>
    /// 应用服务,对外公开的方法
    /// </summary>
    public interface IAppService: IPerRequestDependency, IRemoteService
    {
    }
}
