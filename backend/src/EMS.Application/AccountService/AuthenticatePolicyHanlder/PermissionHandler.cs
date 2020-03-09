using Mbp.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Mbp.Authentication;

namespace EMS.Application.AuthenticatePolicyHanlder
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        public IAuthenticationSchemeProvider Schemes { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public PermissionHandler(IAuthenticationSchemeProvider schemes, IHttpContextAccessor httpContextAccessor)
        {
            Schemes = schemes;
            HttpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var httpContext = HttpContextAccessor.HttpContext;

            var isAuthenticated = httpContext.User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                var routeContext = (context.Resource as RouteEndpoint);

                // 获取Action名称 对用Userclaims 用户鉴定权限点
                var actionName = routeContext.RoutePattern.Defaults.GetValueOrDefault("action");

                // TODO 鉴权 

                // 1. 菜单权限 2.操作权限

                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
