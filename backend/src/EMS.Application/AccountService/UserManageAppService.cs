using AutoMapper;
using Mbp.AspNetCore.Mvc.Convention;
using Mbp.Core.Core;
using Mbp.Core.Modularity;
using Mbp.EntityFrameworkCore.PermissionModel;
using EMS.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Mbp.Ddd.Application.Mbp.UI;
using Mbp.Ddd.Application.System.Linq;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;
using EMS.Application.Contracts.AccountService;
using EMS.Application.Contracts.AccountService.Dto;
using EMS.Application.Contracts.AccountService.DtoSearch;
using Mbp.EntityFrameworkCore.PermissionModel.Enums;

namespace EMS.Application.AccountService
{
    [Authorize(Roles = "admin")]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class UserManageAppService : IUserManageAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        private readonly IConfiguration _config;

        public UserManageAppService(DefaultDbContext defaultDbContext, IConfiguration config)
        {
            _defaultDbContext = defaultDbContext;
            _config = config;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        [HttpPost("AddUser")]
        public virtual int AddUser(UserInputDto userInputDto)
        {
            var user = _mapper.Map<MbpUser>(userInputDto);
            user.UserStatus = EnumUserStatus.Actived;
            user.Password = ApplicationHelper.EncryptPwdMd5(userInputDto.Password);

            _defaultDbContext.MbpUsers.Add(user);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userInputDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateUser")]
        public virtual int UpdateUser(UserInputDto userInputDto)
        {
            // todo 写入mbpuserdept表 mbpuser表冗余部门信息
            var user = _defaultDbContext.MbpUsers.Where(u => u.Id == userInputDto.Id).FirstOrDefault();

            user.IsAdmin = userInputDto.IsAdmin;
            user.PhoneNumber = userInputDto.PhoneNumber;
            user.UserName = userInputDto.UserName;
            user.Code = userInputDto.Code;
            user.Email = userInputDto.Email;
            

            _defaultDbContext.Attach(user);

            _defaultDbContext.MbpUsers.Update(user);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        [HttpPut("RestPwd")]
        public virtual int RestPwd(string loginName, string pwd)
        {
            var user = _defaultDbContext.MbpUsers.Where(u => u.LoginName == loginName).FirstOrDefault();
            user.Password = ApplicationHelper.EncryptPwdMd5(pwd);
            _defaultDbContext.Attach(user);
            _defaultDbContext.MbpUsers.Update(user);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteUser")]
        public virtual int DeleteUser(int userId)
        {
            var user = _defaultDbContext.MbpUsers.Where(u => u.Id == userId).Include(u => u.UserRoles).FirstOrDefault();
            _defaultDbContext.MbpUsers.Remove(user);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        public virtual async Task<PagedList<UserOutputDto>> GetUsers(SearchOptions<UserSearchOptions> searchOptions)
        {
            int total = 0;

            var users = _defaultDbContext.MbpUsers.Include(u => u.UserRoles).PageByAscending(searchOptions.PageSize, searchOptions.PageIndex, out total,
                (c) =>
            c.UserName.Contains(searchOptions.Search.UserName == null ? "" : searchOptions.Search.UserName) &&
            c.LoginName.Contains(searchOptions.Search.LoginName == null ? "" : searchOptions.Search.LoginName) &&
           (!string.IsNullOrEmpty(searchOptions.Search.IsAdmin) ? c.IsAdmin == (searchOptions.Search.IsAdmin == "Y" ? true : false) : true), (c => c.Id)).ToList();

            return new PagedList<UserOutputDto>()
            {
                Content = _mapper.Map<List<UserOutputDto>>(users),
                PageIndex = searchOptions.PageIndex,
                PageSize = searchOptions.PageSize,
                Total = total
            };
        }

        /// <summary>
        /// 配置用户角色
        /// <paramref name="userId"/>
        /// <paramref name="roleIds"/>
        /// </summary>
        [HttpPost("AddUserRoles")]
        public virtual int AddUserRoles(int userId, List<int> roleIds)
        {
            // 查询已有的用户角色
            var mbpUserRoles = _defaultDbContext.MbpUserRoles.Where(ur => roleIds.Contains(ur.RoleId));
            var inserts = roleIds.Except(mbpUserRoles.Select(ur => ur.RoleId));
            foreach (var item in inserts)
            {
                _defaultDbContext.MbpUserRoles.Add(new MbpUserRole() { UserId = userId, RoleId = item });
            }
            var deletes = mbpUserRoles.Select(ur => ur.RoleId).Except(roleIds);
            foreach (var item in deletes)
            {
                _defaultDbContext.MbpUserRoles.Remove(mbpUserRoles.First(ur => ur.RoleId == item));
            }
            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 删除用户角色关系,全部
        /// </summary>
        /// <param name="userId"></param>
        [HttpDelete("DeleteUserRoles")]
        public virtual int DeleteUserRoles(int userId)
        {
            var userRoles = _defaultDbContext.MbpUserRoles.Where(ur => ur.UserId == userId).ToList();

            _defaultDbContext.MbpUserRoles.RemoveRange(userRoles);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 删除用户角色关系,单条
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="RoleId"></param>
        [HttpDelete("DeleteUserRole")]
        public virtual int DeleteUserRole(int userId, int roleId)
        {
            var userRole = _defaultDbContext.MbpUserRoles.Where(ur => ur.UserId == userId && ur.RoleId == roleId).FirstOrDefault();

            _defaultDbContext.MbpUserRoles.Remove(userRole);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 获取用户角色信息
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserRoles")]
        public virtual List<UserRoleOutputDto> GetUserRoles(int userId, string systemCode)
        {
            var userRoles = _defaultDbContext.MbpUserRoles.Where(ur => ur.UserId == userId && ur.Role.SystemCode == systemCode)
                .Include(ur => ur.Role)
                .ToList();

            List<UserRoleOutputDto> userRoleOutputDtos = new List<UserRoleOutputDto>();

            userRoles.ForEach(e =>
            {
                userRoleOutputDtos.Add(new UserRoleOutputDto()
                {
                    UserId = e.UserId,
                    RoleId = e.RoleId,
                    RoleCode = e.Role.Code,
                    RoleName = e.Role.Name,
                    SystemCode = e.Role.SystemCode
                });
            });

            return userRoleOutputDtos;
        }
    }
}
