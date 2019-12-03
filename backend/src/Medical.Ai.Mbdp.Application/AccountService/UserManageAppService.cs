using AutoMapper;
using Mbp.AspNetCore.Mvc.Convention;
using Mbp.Core.Core;
using Mbp.Core.Modularity;
using Mbp.EntityFrameworkCore.PermissionModel;
using Medical.Ai.Mbdp.Application.AccountService.Dto;
using Medical.Ai.Mbdp.EntityFrameworkCore.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Medical.Ai.Mbdp.Application.AccountService
{
    [Authorize(Roles = "admin")]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class UserManageAppService : IUserManageAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        public UserManageAppService(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        [HttpPost("AddUser")]
        public int AddUser(UserInputDto userInputDto)
        {
            var user = _mapper.Map<MbpUser>(userInputDto);

            _defaultDbContext.MbpUsers.Add(user);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        public async Task<List<UserOutputDto>> GetUsers()
        {
            List<MbpUser> users = await _defaultDbContext.MbpUsers.Include(u => u.UserRoles).ToListAsync();

            return _mapper.Map<List<UserOutputDto>>(users);
        }

        /// <summary>
        /// 配置用户角色
        /// <paramref name="userId"/>
        /// <paramref name="roleIds"/>
        /// </summary>
        [HttpPost("AddUserRoles")]
        public int AddUserRoles(int userId, List<int> roleIds)
        {
            // 查询已有的用户角色

            DeleteUserRoles(userId);

            List<MbpUserRole> mbpUserRoles = new List<MbpUserRole>();

            foreach (var roleId in roleIds)
            {
                mbpUserRoles.Add(new MbpUserRole() { UserId = userId, RoleId = roleId });
            }

            _defaultDbContext.MbpUserRoles.AddRange(mbpUserRoles);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 删除用户角色关系,全部
        /// </summary>
        /// <param name="userId"></param>
        [HttpDelete("DeleteUserRoles")]
        public int DeleteUserRoles(int userId)
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
        public int DeleteUserRole(int userId, int roleId)
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
        public List<UserRoleOutputDto> GetUserRoles(int userId)
        {
            var userRoles = _defaultDbContext.MbpUserRoles.Where(ur => ur.UserId == userId)
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
                    RoleName = e.Role.Name
                });
            });

            return userRoleOutputDtos;
        }
    }
}
