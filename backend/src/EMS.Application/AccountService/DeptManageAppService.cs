using AutoMapper;
using EMS.Application.Contracts.AccountService;
using EMS.Application.Contracts.AccountService.Dto;
using EMS.Application.Contracts.AccountService.DtoSearch;
using EMS.EntityFrameworkCore.EntityFrameworkCore;
using Mbp.AspNetCore.Http.Context;
using Mbp.AspNetCore.Mvc.Convention;
using Mbp.Core.Core;
using Mbp.Core.Modularity;
using Mbp.Ddd.Application.Mbp.UI;
using Mbp.Ddd.Application.System.Linq;
using Mbp.EntityFrameworkCore.PermissionModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.AccountService
{
    //[Authorize(Roles = "admin")]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class DeptManageAppService : IDeptManageAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;
        private readonly HttpUserContext _currentUser = null;

        public DeptManageAppService(DefaultDbContext defaultDbContext, HttpUserContext currentUser)
        {
            _defaultDbContext = defaultDbContext;
            _currentUser = currentUser;
        }

        [HttpPost("AddDept")]
        public int AddDept(DeptInputDto deptInputDto)
        {
            // 判断名称是否重复
            if (_defaultDbContext.MbpDepts.Where(d => d.DeptName == deptInputDto.DeptName).Any())
            {
                throw new Exception("部门名称重复");
            }

            var parentDept = _defaultDbContext.MbpDepts.Where(m => m.Id == deptInputDto.ParentId).FirstOrDefault();

            deptInputDto.ParentDeptCode = parentDept.DeptCode;
            deptInputDto.FullDeptName = parentDept.FullDeptName + "/" + deptInputDto.DeptName;
            deptInputDto.ParentDeptName = parentDept.DeptName;

            var dept = _mapper.Map<MbpDept>(deptInputDto);
            dept.SystemCode = "Mbp";
            dept.Level = parentDept.Level + 1;

            _defaultDbContext.MbpDepts.Add(dept);

            return _defaultDbContext.SaveChanges();
        }

        [HttpPut("UpdateDept")]
        public int UpdateDept(DeptInputDto deptInputDto)
        {
            var dept = _mapper.Map<MbpDept>(deptInputDto);

            _defaultDbContext.Attach(dept);

            // 重新继承父级信息, todo优化 可以将此逻辑放到实体里面,当作领域逻辑
            // 刷新当前节点的所有子节点信息
            var parentDept = _defaultDbContext.MbpDepts.Where(m => m.Id == deptInputDto.ParentId).FirstOrDefault();
            dept.SystemCode = parentDept.SystemCode;
            dept.FullDeptName = string.Concat(parentDept.FullDeptName, "/", deptInputDto.DeptName);
            dept.Level = deptInputDto.Level + 1;
            dept.ParentDeptCode = parentDept.DeptCode;
            dept.ParentDeptName = parentDept.DeptName;

            _defaultDbContext.MbpDepts.Update(dept);

            return _defaultDbContext.SaveChanges();
        }

        [HttpDelete("DeleteDept")]
        public int DeleteDept(int deptId)
        {
            var dept = _defaultDbContext.MbpDepts.Where(m => m.Id == deptId).First();

            var depts = _defaultDbContext.MbpDepts.Where(m => m.FullDeptName.StartsWith(dept.FullDeptName));

            _defaultDbContext.MbpDepts.RemoveRange(depts);

            return _defaultDbContext.SaveChanges();
        }

        [HttpGet("GetDepts")]
        public async Task<PagedList<DeptOutputDto>> GetDepts(SearchOptions<DeptSearchOptions> searchOptions)
        {
            int total = 0;

            List<MbpDept> depts = _defaultDbContext.MbpDepts.PageByAscending(searchOptions.PageSize, searchOptions.PageIndex, out total, (c) =>
            c.DeptName.Contains(searchOptions.Search.Name == null ? "" : searchOptions.Search.Name) &&
            c.DeptCode.Contains(searchOptions.Search.Code == null ? "" : searchOptions.Search.Code),
            (c => c.Id)).ToList();

            var content = _mapper.Map<List<DeptOutputDto>>(depts);

            var dictNodes = content.ToDictionary(x => x.Id);

            List<DeptOutputDto> result = new List<DeptOutputDto>();
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
            return new PagedList<DeptOutputDto>()
            {
                Content = result,
                PageIndex = searchOptions.PageIndex,
                PageSize = searchOptions.PageSize,
                Total = total
            };
        }
    }
}
