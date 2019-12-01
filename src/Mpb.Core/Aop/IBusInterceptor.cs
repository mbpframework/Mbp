using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbp.Core.Aop
{
    /// <summary>
    /// 是否启用拦截器
    /// </summary>
    public interface IBusInterceptor : IInterceptor
    {
        /*所有扩展的拦截器必须实现此接口,
         * 而不得直接实现IInterceptor,
         * 否则框架扫描不到拦截器*/
    }
}
