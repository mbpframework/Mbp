using Mbp.Core.Core;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mbp.AspNetCore.Mvc.Filter
{
    public class MbpExceptionFilter : IAsyncActionFilter
    {
        private ILogger<MbpExceptionFilter> _logger;

        public MbpExceptionFilter(ILogger<MbpExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

            var methodInfo = actionDescriptor.MethodInfo;

            //必须显示在服务类上定义AutoAopAttribute特性
            if (methodInfo.DeclaringType.IsDefined(typeof(AutoAopAttribute), true)
                && ((AutoAopAttribute)methodInfo.DeclaringType.GetCustomAttributes(typeof(AutoAopAttribute), true)[0]).IsAutoCatchEx)
            {
                //可以通过在方法上定义特性头来避开拦截 比如[AutoAop(IsAutoCatchEx=false)]
                if ((methodInfo.IsDefined(typeof(AutoAopAttribute), true)
                    && !((AutoAopAttribute)methodInfo.GetCustomAttributes(typeof(AutoAopAttribute), false)[0]).IsAutoCatchEx)
                    || (methodInfo.IsDefined(typeof(AutoAopAttribute), true)
                    && !((AutoAopAttribute)methodInfo.GetCustomAttributes(typeof(AutoAopAttribute), false)[0]).IsAutoCatchEx))
                {
                    await next();
                }
                else
                {
                    try
                    {
                        await next();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"{methodInfo.Name}方法发生[{ex.Message}]异常", ex);
                    }
                }
            }
            else
            {
                await next();
            }
        }
    }
}
