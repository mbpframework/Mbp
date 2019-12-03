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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Medical.Ai.Mbdp.Application.AccountService
{
    [Authorize(Roles = "admin")]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class RoleManageAppService : IRoleManageAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        public RoleManageAppService(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        [HttpPost("AddRole")]
        public void AddRole(RoleInputDto roleInputDto)
        {
            var role = _mapper.Map<MbpRole>(roleInputDto);

            _defaultDbContext.MbpRoles.Add(role);

            _defaultDbContext.SaveChanges();
        }

        [HttpGet("GetRoles")]
        public async Task<List<RoleOutputDto>> GetRoles()
        {
            List<MbpRole> roles = await _defaultDbContext.MbpRoles.Include(u => u.RoleMenus).ToListAsync();

            return _mapper.Map<List<RoleOutputDto>>(roles);
        }

        /// <summary>
        /// 配置角色功能
        /// </summary>
        [HttpPost("AddRoleMenus")]
        public int AddRoleMenus(int roleId, List<int> menuIds)
        {
            // 查询已有的用户角色

            DeleteRoleMenus(roleId);

            List<MbpRoleMenu> mbpRoleMenus = new List<MbpRoleMenu>();

            foreach (var menuId in menuIds)
            {
                mbpRoleMenus.Add(new MbpRoleMenu() { RoleId = roleId, MenuId = menuId });
            }

            _defaultDbContext.MbpRoleMenus.AddRange(mbpRoleMenus);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 删除角色功能关系,全部
        /// </summary>
        /// <param name="roleId"></param>
        [HttpDelete("DeleteRoleMenus")]
        public int DeleteRoleMenus(int roleId)
        {
            var userRoles = _defaultDbContext.MbpRoleMenus
                .Where(rm => rm.RoleId == roleId).ToList();

            _defaultDbContext.MbpRoleMenus.RemoveRange(userRoles);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 删除角色功能关系,单条
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="menuId"></param>
        [HttpDelete("DeleteRoleMenu")]
        public int DeleteRoleMenu(int roleId, int menuId)
        {
            var roleMenu = _defaultDbContext.MbpRoleMenus
                .Where(rm => rm.RoleId == roleId && rm.MenuId == menuId)
                .FirstOrDefault();

            _defaultDbContext.MbpRoleMenus.Remove(roleMenu);

            return _defaultDbContext.SaveChanges();
        }
    }
}
