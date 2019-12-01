using Castle.DynamicProxy;
using Mbp.Core.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbp.Core.Aop
{
    /// <summary>
    /// 日志拦截器
    /// </summary>
    public class LogInterceptor : IBusInterceptor
    {
        public virtual void Intercept(IInvocation invocation)
        {
            //必须显示在服务类上定义AutoAopAttribute特性
            if (invocation.TargetType.IsDefined(typeof(AutoAopAttribute), true)
                && ((AutoAopAttribute)invocation.TargetType.GetCustomAttributes(typeof(AutoAopAttribute), true)[0]).IsAutoLog)
            {
                //可以通过在方法上定义特性头来避开拦截 比如[AutoAop(IsAutoLog=false)]
                if ((invocation.MethodInvocationTarget.IsDefined(typeof(AutoAopAttribute), true)
                    && !((AutoAopAttribute)invocation.MethodInvocationTarget.GetCustomAttributes(typeof(AutoAopAttribute), false)[0]).IsAutoLog)
                    || (invocation.Method.IsDefined(typeof(AutoAopAttribute), true)
                    && !((AutoAopAttribute)invocation.Method.GetCustomAttributes(typeof(AutoAopAttribute), false)[0]).IsAutoLog))
                {
                    invocation.Proceed();
                }
                else
                {
                    //LogHelper.Info($"正在执行方法:[{invocation.Method.Name}]");

                    foreach (var args in invocation.Arguments)
                    {
                        //LogHelper.Debug(args.ToString());
                    }

                    //TO DO参数值监控

                    invocation.Proceed();
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
