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
        public virtual int AddDept(DeptInputDto deptInputDto)
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
        public virtual int UpdateDept(DeptInputDto deptInputDto)
        {
            var dept = _mapper.Map<MbpDept>(deptInputDto);

            // 重新继承父级信息, todo优化 可以将此逻辑放到实体里面,当作领域逻辑
            var parentDept = _defaultDbContext.MbpDepts.Where(m => m.Id == deptInputDto.ParentId).FirstOrDefault();

            // 判断选择的上级是不是自己的下级部门,这种选择是不合理的
            if (parentDept.FullDeptName.StartsWith(dept.FullDeptName))
                throw new Exception("不能使用当前下级部门作为父级部门");

            // 刷新当前节点
            dept.SystemCode = parentDept.SystemCode;
            dept.FullDeptName = string.Concat(parentDept.FullDeptName, "/", dept.DeptName);
            dept.Level = parentDept.Level + 1;
            dept.ParentDeptCode = parentDept.DeptCode;
            dept.ParentDeptName = parentDept.DeptName;

            _defaultDbContext.Attach(dept);

            // 刷新下级节点
            var current = _defaultDbContext.MbpDepts.Include("ChildrenDept.ChildrenDept.ChildrenDept.ChildrenDept.ChildrenDept")
                .First(m => m.Id == dept.Id);
            RefreshChildrenInfo(dept, current.ChildrenDept);

            _defaultDbContext.Update(current);
            // 提交所有修改
            return _defaultDbContext.SaveChanges();
        }

        // 刷新部门信息
        private void RefreshChildrenInfo(MbpDept current, List<MbpDept> children)
        {
            children.ForEach(m =>
            {
                m.SystemCode = current.SystemCode;
                m.FullDeptName = string.Concat(current.FullDeptName, "/", m.DeptName);
                m.Level = current.Level + 1;
                m.ParentDeptCode = current.DeptCode;
                m.ParentDeptName = current.DeptName;

                RefreshChildrenInfo(m, m.ChildrenDept);
            });
        }

        [HttpDelete("DeleteDept")]
        public virtual int DeleteDept(int deptId)
        {
            var dept = _defaultDbContext.MbpDepts.Where(m => m.Id == deptId).First();

            var depts = _defaultDbContext.MbpDepts.Where(m => m.FullDeptName.StartsWith(dept.FullDeptName));

            _defaultDbContext.MbpDepts.RemoveRange(depts);

            return _defaultDbContext.SaveChanges();
        }

        [HttpGet("GetDepts")]
        public virtual async Task<PagedList<DeptOutputDto>> GetDepts(SearchOptions<DeptSearchOptions> searchOptions)
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
