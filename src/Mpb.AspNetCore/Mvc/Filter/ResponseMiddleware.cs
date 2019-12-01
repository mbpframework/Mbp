using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.AspNetCore.Mvc.Filter
{
    /// <summary>
    /// 请求响应数据格式处理中间件,支持继承重写
    /// </summary>
    public class ResponseMiddleware : ActionFilterAttribute
    {
        /// <summary>
        /// 格式化响应数据
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is JsonResult)
            {
                var jsonResult = context.Result as JsonResult;

                if (jsonResult.Value == null)
                {
                    context.Result = new JsonResult(new { Code = 404, Message = "未找到资源", Version = context.RouteData.Values["version"], Data = new List<object>() });
                }
                else
                {
                    context.Result = new JsonResult(new { Code = 200, Message = "正常", Version = context.RouteData.Values["version"], Data = jsonResult.Value });
                }
            }
            else if (context.Result is ObjectResult)
            {
                var objectResult = context.Result as ObjectResult;

                if (objectResult.Value == null)
                {
                    context.Result = new JsonResult(new { Code = 404, Message = "未找到资源", Version = context.RouteData.Values["version"], Data = new List<object>() });
                }
                else
                {
                    context.Result = new JsonResult(new { Code = 200, Message = "正常", Version = context.RouteData.Values["version"], Data = objectResult.Value });
                }
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = new ObjectResult(new { Code = 404, Message = "未找到资源", Version = context.RouteData.Values["version"], Data = new List<object>() });
            }
            else if (context.Result is ContentResult)
            {
                context.Result = new ObjectResult(new { Code = 200, Message = "正常", Version = context.RouteData.Values["version"], Data = (context.Result as ContentResult).Content });
            }
            else if (context.Result is StatusCodeResult)
            {
                context.Result = new ObjectResult(new { Code = (context.Result as StatusCodeResult).StatusCode, Message = "", Version = context.RouteData.Values["version"], Data = new List<object>() });
            }
        }
    }
}
