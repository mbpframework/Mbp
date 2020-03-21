using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mbp.AspNetCore.Mvc.Middleware
{
    /// <summary>
    /// 应用层全局异常处理中间件
    /// </summary>
    public class MbpGlobaExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public MbpGlobaExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<MbpGlobaExceptionMiddleware> logger)
        {
            try
            {
                // Call the next delegate/middleware in the pipeline
                await _next(context);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // 发生冲突时候,牺牲后者.不做具体数据合并操作.提示当前用户数据已经发生修改,需要重试.
                logger.LogError("并发冲突:" + ex.Message);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { Code = 500, Message = "提交并发冲突", Version = "1", Data = new List<object>() }));
            }
            catch (Exception ex)
            {
                // 其他异常
                logger.LogError($"请求[{context.Request.Path}]发生异常:" + ex.Message);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { Code = 500, Message = ex.Message, Version = "1", Data = new List<object>() }));
            }
        }
    }
}
