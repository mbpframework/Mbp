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
    /// 异常拦截器
    /// </summary>
    public class ExceptionInterceptor : IBusInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //必须显示在服务类上定义AutoAopAttribute特性
            if (invocation.TargetType.IsDefined(typeof(AutoAopAttribute), true)
                && ((AutoAopAttribute)invocation.TargetType.GetCustomAttributes(typeof(AutoAopAttribute), true)[0]).IsAutoCatchEx)
            {
                //可以通过在方法上定义特性头来避开拦截 比如[AutoAop(IsAutoCatchEx=false)]
                if ((invocation.MethodInvocationTarget.IsDefined(typeof(AutoAopAttribute), true)
                    && !((AutoAopAttribute)invocation.MethodInvocationTarget.GetCustomAttributes(typeof(AutoAopAttribute), false)[0]).IsAutoCatchEx)
                    || (invocation.Method.IsDefined(typeof(AutoAopAttribute), true)
                    && !((AutoAopAttribute)invocation.Method.GetCustomAttributes(typeof(AutoAopAttribute), false)[0]).IsAutoCatchEx))
                {
                    invocation.Proceed();
                }
                else
                {
                    try
                    {
                        invocation.Proceed();
                    }
                    catch (Exception ex)
                    {
                        //LogHelper.Error($"{invocation.Method.Name}方法发生[{ex.Message}]异常", ex);
                    }
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
    }
}
