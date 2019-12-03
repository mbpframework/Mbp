using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Mbp.Core.Core;
using System.Linq;

namespace Mbp.AspNetCore.Mvc.Filter
{
    public class MbpTransActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor))
            {
                await next();
                return;
            }

            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            var isGetMethod = (((Microsoft.AspNetCore.Mvc.ActionConstraints.HttpMethodActionConstraint)actionDescriptor.ActionConstraints[0]).HttpMethods).Any(m => m == "GET");

            var methodInfo = actionDescriptor.MethodInfo;

            // 开启事务条件:公共方法 虚函数 非Get请求 非特定名称开头的函数 特别指明不需要事务
            // TODO 后面根据实际需要进行调整
            if (!methodInfo.IsPublic
                || !methodInfo.IsVirtual
                || !isGetMethod
                || methodInfo.IsDefined(typeof(HttpGetAttribute), true)
                || (methodInfo.IsDefined(typeof(AutoAopAttribute), true) && !((AutoAopAttribute)methodInfo.GetCustomAttributes(typeof(AutoAopAttribute), false)[0]).IsTranstion)
                || methodInfo.Name.StartsWith("Get")
                || methodInfo.Name.StartsWith("Query")
                || methodInfo.Name.StartsWith("Fetch")
                || methodInfo.Name.StartsWith("Find")
                )
            {
                await next();
                return;
            }

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                var result = await next();

                scope.Complete();
            }
        }
    }
}
