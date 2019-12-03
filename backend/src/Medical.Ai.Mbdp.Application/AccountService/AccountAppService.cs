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
using Mbp.EntityFrameworkCore.PermissionModel;

namespace Medical.Ai.Mbdp.Application.AccountService
{
    /// <summary>
    /// 账号管理,相关服务需要管理员权限
    /// </summary>
    [Authorize("GlobalPermission")]
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
            var user = _defaultDbContext.MbpUsers.Where(u => u.LoginName == loginInputDto.LoginName).FirstOrDefault();

            if (user != null)
            {
                // 如果是管理员权限就给管理员属性,如果是用户就给用户属性,这里只定义两种角色,一种是超管,一种是普通用户,这里的角色只做身份鉴定,不做鉴权用
                var token = await _jwtBearerService.CreateJwt(loginInputDto.LoginName, loginInputDto.ClientID, new List<Claim>()
                    {
                       new Claim(ClaimTypes.Role, user.IsAdmin?"admin":"user")
                    });

                // 取出用户的菜单权限
                var menus = (from tuser in _defaultDbContext.Set<MbpUser>()
                             join tuserrole in _defaultDbContext.Set<MbpUserRole>()
                                 on tuser.Id equals tuserrole.UserId
                             join trole in _defaultDbContext.Set<MbpRole>()
                                 on tuserrole.RoleId equals trole.Id
                             join trolemenu in _defaultDbContext.Set<MbpRoleMenu>()
                                 on trole.Id equals trolemenu.RoleId
                             join tmenu in _defaultDbContext.Set<MbpMenu>()
                                 on trolemenu.RoleId equals tmenu.Id
                             select tmenu.Path).ToList();

                return new LoginOutputDto() { AccessToken = token, Menus = menus };
            }

            return new LoginOutputDto() { AccessToken = new Jwt() };
        }

        [AllowAnonymous]
        [HttpGet("RefreshToken")]
        public async Task<Jwt> RefreshToken(string refreshToken)
        {
            return await _jwtBearerService.RefreshJwt(refreshToken);
        }

        [AllowAnonymous]
        [HttpGet("LoginOut")]
        public void LoginOut()
        {

        }
    }
}
