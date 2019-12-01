using System;

namespace Mbp.Core.Core
{
    /// <summary>
    /// 指示类型是否开启日志AOP拦截
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class AutoAopAttribute : Attribute
    {
        /// <summary>
        /// 是否自动记录日志
        /// </summary>
        public bool IsAutoLog { get; set; } = true;

        /// <summary>
        /// 是否自动捕获异常
        /// </summary>
        public bool IsAutoCatchEx { get; set; } = true;

        /// <summary>
        /// 是否开启事务
        /// </summary>
        public bool IsTranstion { get; set; } = true;
    }
}
