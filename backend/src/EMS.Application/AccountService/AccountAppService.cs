using AutoMapper;
using Mbp.AspNetCore.Mvc.Convention;
using Mbp.Authentication.JwtBearer;
using Mbp.Core.Core;
using Mbp.Core.Modularity;
using EMS.EntityFrameworkCore.EntityFrameworkCore;
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
using EMS.Application.Contracts.AccountService;
using EMS.Application.Contracts.AccountService.Dto;
using Microsoft.IdentityModel.JsonWebTokens;
using Mbp.AspNetCore.Http.Context;
using Mbp.EntityFrameworkCore.PermissionModel.Enums;

namespace EMS.Application.AccountService
{
    /// <summary>
    /// 账号管理,相关服务需要管理员权限
    /// </summary>
    [Authorize]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class AccountAppService : IAccountAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        private readonly IJwtBearerService _jwtBearerService = null;

        private readonly HttpUserContext _currentUser = null;

        public AccountAppService(DefaultDbContext defaultDbContext, IJwtBearerService jwtBearerService, HttpUserContext currentUser)
        {
            _defaultDbContext = defaultDbContext;
            _jwtBearerService = jwtBearerService;
            _currentUser = currentUser;
        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public async Task<LoginOutputDto> Login(LoginInputDto loginInputDto)
        {
            var user = _defaultDbContext.MbpUsers.Where(u => u.LoginName == loginInputDto.LoginName).FirstOrDefault();

            if (user != null)
            {
                if (user.Password != ApplicationHelper.EncryptPwdMd5(loginInputDto.Password))
                {
                    return new LoginOutputDto() { AccessToken = new Jwt(), IsPassPwdCheck = false };
                }

                // 如果是管理员权限就给管理员属性,如果是用户就给用户属性,这里只定义两种角色,一种是超管,一种是普通用户,这里的角色只做身份鉴定,不做鉴权用,方便区分超管类的API和普通用户的API
                // 安全级别较高的API需要超管的可以单独限定,其他按照全局授权策略来进行
                // token会默认添加用户名和登录名,现在根据需要,在这里扩展更多的扩展信息,加入角色名称,用户Id
                var token = await _jwtBearerService.CreateJwt(loginInputDto.LoginName, loginInputDto.ClientID, new List<Claim>()
                    {
                       new Claim(ClaimTypes.Role, user.IsAdmin?"admin":"user"),// 角色名称,这里的ClaimTypes.Role,可以用来约束基于角色的权限验证
                       new Claim(ClaimTypes.Sid, user.Id.ToString()) // 登录名
                    });

                return new LoginOutputDto() { AccessToken = token, Menus = new List<string>(), UserName = user.UserName, Role = user.IsAdmin ? "admin" : "user", IsPassPwdCheck = true };
            }

            return new LoginOutputDto() { AccessToken = new Jwt(), IsPassPwdCheck = false };
        }

        /// <summary>
        /// 提供给前端的个人用户信息,实时
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserInfo")]
        public async Task<UserOutputDto> GetUserInfo()
        {
            UserOutputDto userInfo = new UserOutputDto();

            var user = _defaultDbContext.MbpUsers.Where(u => u.LoginName == _currentUser.LoginName).FirstOrDefault();

            userInfo.Email = _currentUser.Email;
            userInfo.Code = user.Code;
            userInfo.Id = _currentUser.Id;
            userInfo.IsAdmin = user.IsAdmin;
            userInfo.LoginName = _currentUser.LoginName;
            userInfo.PhoneNumber = "";
            userInfo.UserAvatar = "";
            userInfo.UserName = _currentUser.UserName;
            userInfo.UserStatus = user.UserStatus;

            return userInfo;
        }

        /// <summary>
        /// 获取左侧菜单栏的路由菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMenusForRoute")]
        public async Task<List<RouteOutputDto>> GetMenusForRoute()
        {
            List<RouteOutputDto> routeOutputDtos = new List<RouteOutputDto>();
            List<MbpMenu> menus;
            // 过滤超管,如果是超管admin账号则忽略,为了防止菜单被其他管理员误操作导致不好复原
            if (_currentUser.LoginName == "admin")
            {
                menus = _defaultDbContext.MbpMenus.Where(m => m.MenuType == EnumMenuType.Page).ToList();
            }
            else
            {
                // 其他用户 根据用户角色权限的配置获取菜单
                menus = (from tuser in _defaultDbContext.Set<MbpUser>()
                         join tuserrole in _defaultDbContext.Set<MbpUserRole>()
                             on tuser.Id equals tuserrole.UserId
                         join trole in _defaultDbContext.Set<MbpRole>()
                             on tuserrole.RoleId equals trole.Id
                         join trolemenu in _defaultDbContext.Set<MbpRoleMenu>()
                             on trole.Id equals trolemenu.RoleId
                         join tmenu in _defaultDbContext.Set<MbpMenu>()
                             on trolemenu.MenuId equals tmenu.Id
                         where tmenu.MenuType == EnumMenuType.Page && tuser.Id == _currentUser.Id
                         select tmenu).ToList();
            }
            List<RouteOutputDto> tempRoute = new List<RouteOutputDto>();

            // 将菜单调整为路由表数据结构形式
            foreach (var item in menus)
            {
                RouteOutputDto routeOutputDto = _mapper.Map<RouteOutputDto>(item);
                routeOutputDto.Name = routeOutputDto.Code;
                routeOutputDto.Component = item.MenuCompent;
                routeOutputDto.Meta.Title = item.Name;
                routeOutputDto.Meta.Icon = item.MenuIcon;

                tempRoute.Add(routeOutputDto);
            }

            var dictNodes = tempRoute.ToDictionary(x => x.Id);
            // 重组成vue-element-admin 路由表json
            foreach (var item in dictNodes.Values)
            {
                if (item.ParentId == 1)
                {
                    item.Path = "/" + item.Path;
                    routeOutputDtos.Add(item);
                }
                else
                {
                    if (dictNodes.ContainsKey(item.ParentId))
                    {
                        dictNodes[item.ParentId].Children.Add(item);
                    }
                }
            }

            return routeOutputDtos;
        }

        [AllowAnonymous]
        [HttpGet("GetToken")]
        public async Task<LoginOutputDto> GetToken(string loginName)
        {
            var clientID = Guid.NewGuid().ToString();
            var user = _defaultDbContext.MbpUsers.Where(u => u.LoginName == loginName).FirstOrDefault();

            if (user != null)
            {
                // 如果是管理员权限就给管理员属性,如果是用户就给用户属性,这里只定义两种角色,一种是超管,一种是普通用户,这里的角色只做身份鉴定,不做鉴权用
                var token = await _jwtBearerService.CreateJwt(loginName, clientID, new List<Claim>()
                    {
                       new Claim(ClaimTypes.Role, user.IsAdmin?"admin":"user"),
                       new Claim(ClaimTypes.Sid, user.Id.ToString()) // 登录名
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

                return new LoginOutputDto() { AccessToken = token, Menus = menus, UserName = user.UserName, Role = user.IsAdmin ? "admin" : "user", IsPassPwdCheck = true };
            }

            return new LoginOutputDto() { AccessToken = new Jwt(), IsPassPwdCheck = false };
        }

        [AllowAnonymous]
        [HttpGet("RefreshToken")]
        public async Task<Jwt> RefreshToken(string refreshToken)
        {
            return await _jwtBearerService.RefreshJwt(refreshToken);
        }

        [AllowAnonymous]
        [HttpGet("LogOut")]
        public int LogOut()
        {
            return 0;
        }
    }
}
