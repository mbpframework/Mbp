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
        public void AddMenu(MenuInputDto menuInputDto)
        {
            var menu = _mapper.Map<MbpMenu>(menuInputDto);

            _defaultDbContext.MbpMenus.Add(menu);

            _defaultDbContext.SaveChanges();
        }

        [HttpGet("GetMenus")]
        public async Task<List<MenuOutputDto>> GetMenus()
        {
            List<MbpMenu> menus = await _defaultDbContext.MbpMenus.Include(u => u.MenuClaims).ToListAsync();

            return _mapper.Map<List<MenuOutputDto>>(menus);
        }

        /// <summary>
        /// 配置功能操作权限
        /// </summary>
        [HttpPost("AddMenuClaims")]
        public void AddMenuClaims()
        {

        }
    }
}
