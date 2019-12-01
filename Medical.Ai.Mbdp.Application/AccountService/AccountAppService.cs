using AutoMapper;
using Mbp.AspNetCore.Mvc.Convention;
using Mbp.Authentication.JwtBearer;
using Mbp.Core.Core;
using Mbp.Core.Modularity;
using Medical.Ai.Mbdp.Application.AccountService.Dto;
using Medical.Ai.Mbdp.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Medical.Ai.Mbdp.Application.AccountService
{
    /// <summary>
    /// 账号管理,相关服务需要管理员权限
    /// </summary>
    [Authorize(Roles = "administator")]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class AccountAppService : IAccountAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        private readonly IJwtBearerService _jwtBearerService = null;

        public AccountAppService(DefaultDbContext defaultDbContext, IJwtBearerService jwtBearerService)
        {
            _defaultDbContext = defaultDbContext;
            _jwtBearerService = jwtBearerService;
        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public async Task<LoginOutputDto> Login(LoginInputDto loginInputDto)
        {
            var user = await _defaultDbContext.MbpUsers.FindAsync(loginInputDto.LoginName);

            if (user != null)
            {
                var userRole = _defaultDbContext.MbpUserRoles.Where(r => r.UserId == user.Id).FirstOrDefault();
                if (userRole != null)
                {
                    // 如果是管理员权限就给管理员属性,如果是用户就给用户属性,这里只定义两种角色,一种是超管,一种是普通用户,这里的角色只做身份鉴定,不做鉴权用
                    var token = await _jwtBearerService.CreateJwt("123", "lixp", new List<Claim>()
                    {
                       new Claim(ClaimTypes.Role, "administator")
                    });

                    // 取出用户的菜单权限
                    var menus = new List<string>();

                    return new LoginOutputDto() { AccessToken = token, Menus = menus };
                }
            }

            return new LoginOutputDto() { AccessToken = new Jwt() };
        }

        // todo 添加登出, 添加用户,添加角色,添加用户角色,添加角色菜单等功能
    }
}
