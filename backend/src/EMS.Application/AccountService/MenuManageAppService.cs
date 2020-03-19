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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Mbp.Ddd.Application.System.Linq;
using Mbp.Ddd.Application.Mbp.UI;
using EMS.Application.Contracts.AccountService;
using EMS.Application.Contracts.AccountService.Dto;
using EMS.Application.Contracts.AccountService.DtoSearch;
using Mbp.AspNetCore.Http.Context;
using Mbp.EntityFrameworkCore.PermissionModel.Enums;

namespace EMS.Application.AccountService
{
    [Authorize(Roles = "admin")]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class MenuManageAppService : IMenuManageAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;
        private readonly HttpUserContext _currentUser = null;

        public MenuManageAppService(DefaultDbContext defaultDbContext, HttpUserContext currentUser)
        {
            _defaultDbContext = defaultDbContext;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 添加功能菜单
        /// </summary>
        [HttpPost("AddMenu")]
        public int AddMenu(MenuInputDto menuInputDto)
        {
            var parentMenu = _defaultDbContext.MbpMenus.Where(m => m.Id == menuInputDto.ParentId).FirstOrDefault();

            menuInputDto.CodePath = string.Concat(parentMenu.CodePath, "/", menuInputDto.Code);
            menuInputDto.Level = parentMenu.Level + 1;
            menuInputDto.SystemCode = parentMenu.SystemCode;

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

            // 重新继承父级信息, to do优化
            var parentMenu = _defaultDbContext.MbpMenus.Where(m => m.Id == menuInputDto.ParentId).FirstOrDefault();
            menu.SystemCode = parentMenu.SystemCode;
            menu.CodePath = string.Concat(parentMenu.CodePath, "/", menuInputDto.Code);

            _defaultDbContext.MbpMenus.Update(menu);

            return _defaultDbContext.SaveChanges();
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMenus")]
        public async Task<PagedList<MenuOutputDto>> GetMenus(SearchOptions<MenuSearchOptions> searchOptions)
        {
            int total = 0;

            List<MbpMenu> menus = _defaultDbContext.MbpMenus.Include(u => u.MenuClaims).PageByAscending(searchOptions.PageSize, searchOptions.PageIndex, out total, (c) =>
            c.Name.Contains(searchOptions.Search.Name == null ? "" : searchOptions.Search.Name) &&
            c.Code.Contains(searchOptions.Search.Code == null ? "" : searchOptions.Search.Code) &&
           (!string.IsNullOrEmpty(searchOptions.Search.SystemCode) ? (c.SystemCode == searchOptions.Search.SystemCode || c.Id == 1) : true),
            (c => c.Id)).ToList();

            var content = _mapper.Map<List<MenuOutputDto>>(menus);

            var dictNodes = content.ToDictionary(x => x.Id);

            List<MenuOutputDto> result = new List<MenuOutputDto>();
            foreach (var item in dictNodes.Values)
            {
                if (item.ParentId == 0)
                {
                    result.Add(item);
                }
                else
                {
                    if (dictNodes.ContainsKey(item.ParentId))
                    {
                        dictNodes[item.ParentId].Children.Add(item);
                    }
                }
            }

            // 返回列表分页数据
            return new PagedList<MenuOutputDto>()
            {
                Content = result,
                PageIndex = searchOptions.PageIndex,
                PageSize = searchOptions.PageSize,
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
            var menu = _defaultDbContext.MbpMenus.Where(m => m.Id == menuId).First();

            var menus = _defaultDbContext.MbpMenus.Include(m => m.MenuClaims).Where(m => m.CodePath.StartsWith(menu.CodePath));

            _defaultDbContext.MbpMenus.RemoveRange(menus);

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
