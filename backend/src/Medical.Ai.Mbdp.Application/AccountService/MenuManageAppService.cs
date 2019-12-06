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
using Mbp.Ddd.Application.System.Linq;
using Mbp.Ddd.Application.Mbp.UI;

namespace Medical.Ai.Mbdp.Application.AccountService
{
    [Authorize(Roles = "admin")]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class MenuManageAppService : IMenuManageAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        public MenuManageAppService(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
        }

        /// <summary>
        /// 添加功能菜单
        /// </summary>
        [HttpPost("AddMenu")]
        public int AddMenu(MenuInputDto menuInputDto)
        {
            var menu = _mapper.Map<MbpMenu>(menuInputDto);

            _defaultDbContext.MbpMenus.Add(menu);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="menuInputDto"></param>
        /// <returns></returns>
        [HttpPut("UpdateMenu")]
        public int UpdateMenu(MenuInputDto menuInputDto)
        {
            var menu = _mapper.Map<MbpMenu>(menuInputDto);

            _defaultDbContext.Attach(menu);

            _defaultDbContext.MbpMenus.Update(menu);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMenus")]
        public async Task<PagedList<MenuOutputDto>> GetMenus(int pageSize, int pageIndex)
        {
            int total = 0;

            List<MbpMenu> menus = _defaultDbContext.MbpMenus.Include(u => u.MenuClaims).PageByAscending(pageSize, pageIndex, out total, (c) => true, (c => c.Id)).ToList();

            // 返回列表分页数据
            return new PagedList<MenuOutputDto>()
            {
                Content = _mapper.Map<List<MenuOutputDto>>(menus),
                PageIndex = pageIndex,
                PageSize = pageSize,
                Total = total
            };
        }

        /// <summary>
        /// 配置功能操作权限
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        [HttpPost("AddMenuClaims")]
        public int AddMenuClaims(int menuId, List<MenuClaimInputDto> claims)
        {
            // 删除Claims
            DeleteMenuClaims(menuId);

            List<MbpMenuClaim> menuClaims = new List<MbpMenuClaim>();

            foreach (var claim in claims)
            {
                menuClaims.Add(new MbpMenuClaim()
                {
                    MenuId = menuId,
                    ClaimType = claim.ClaimType,
                    ClaimValue = claim.ClaimValue
                });
            }

            _defaultDbContext.MbpMenuClaims.AddRange(menuClaims);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 删除菜单,单条
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMenu")]
        public int DeleteMenu(int menuId)
        {
            var menu = _defaultDbContext.MbpMenus.Include(m => m.MenuClaims).Where(m => m.Id == menuId).First();
            _defaultDbContext.MbpMenus.Remove(menu);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 删除菜单,多条
        /// </summary>
        /// <param name="menuIds"></param>
        /// <returns></returns>
        [HttpDelete("DeleteMenus")]
        public int DeleteMenus(List<int> menuIds)
        {
            var menus = _defaultDbContext.MbpMenus.Include(m => m.MenuClaims).Where(m => menuIds.Contains(m.Id));
            _defaultDbContext.MbpMenus.RemoveRange(menus);

            return _defaultDbContext.SaveChanges();
        }

        private int DeleteMenuClaims(int menuId)
        {
            var menuClaims = _defaultDbContext.MbpMenuClaims.Where(c => c.MenuId == menuId);

            _defaultDbContext.MbpMenuClaims.RemoveRange(menuClaims);

            return _defaultDbContext.SaveChanges();
        }
    }
}
