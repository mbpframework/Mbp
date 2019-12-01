using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Threading.Tasks;

namespace Mbp.AspNetCore.Mvc.Filter
{
    public class MbpAuthorizationFilter : AuthorizeFilter
    {
        //public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        //{
        //    return base.OnAuthorizationAsync(context);
        //}
    }

    public class AllowAnonymous : AuthorizeFilter, IAllowAnonymousFilter
    {
        //public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        //{
        //    return base.OnAuthorizationAsync(context);
        //}
    }
}
