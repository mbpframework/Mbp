using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Castle.DynamicProxy;
using Mbp.Core.Core;

namespace Mbp.Core.Aop
{
    public class TransactionInterceptor : IBusInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            //必须显示在服务类上定义AutoAopAttribute特性
            if (invocation.TargetType.IsDefined(typeof(AutoAopAttribute), true)
                && ((AutoAopAttribute)invocation.TargetType.GetCustomAttributes(typeof(AutoAopAttribute), true)[0]).IsTranstion)
            {
                //可以通过在方法上定义特性头来避开拦截 比如[AutoAop(IsAutoLog=false)]
                if ((invocation.MethodInvocationTarget.IsDefined(typeof(AutoAopAttribute), true)
                    && !((AutoAopAttribute)invocation.MethodInvocationTarget.GetCustomAttributes(typeof(AutoAopAttribute), false)[0]).IsTranstion)
                    || (invocation.Method.IsDefined(typeof(AutoAopAttribute), true)
                    && !((AutoAopAttribute)invocation.Method.GetCustomAttributes(typeof(AutoAopAttribute), false)[0]).IsTranstion))
                {
                    invocation.Proceed();
                }
                else
                {
                    using (var scope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        invocation.Proceed();

                        scope.Complete();
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
