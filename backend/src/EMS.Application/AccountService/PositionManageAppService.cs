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
using Mbp.EntityFrameworkCore.Domain;
using Mbp.EntityFrameworkCore.Domain.Enums;
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
    public class PositionManageAppService : IPositionManageAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;
        private readonly HttpUserContext _currentUser = null;

        public PositionManageAppService(DefaultDbContext defaultDbContext, HttpUserContext currentUser)
        {
            _defaultDbContext = defaultDbContext;
            _currentUser = currentUser;
        }

        [HttpPost("AddPosition")]
        public virtual int AddPosition(PositionInputDto PositionInputDto)
        {
            // 判断名称是否重复
            if (_defaultDbContext.MbpPositions.Where(d => d.PositionName == PositionInputDto.PositionName).Any())
            {
                throw new Exception("岗位名称重复");
            }

            var parentPosition = _defaultDbContext.MbpPositions.Where(m => m.Id == PositionInputDto.ParentId).FirstOrDefault();

            PositionInputDto.ParentPositionCode = parentPosition.PositionCode;
            PositionInputDto.FullPositionName = parentPosition.FullPositionName + "/" + PositionInputDto.PositionName;
            PositionInputDto.ParentPositionName = parentPosition.PositionName;

            var Position = _mapper.Map<MbpPosition>(PositionInputDto);
            Position.SystemCode = "Mbp";
            Position.Level = parentPosition.Level + 1;

            _defaultDbContext.MbpPositions.Add(Position);

            return _defaultDbContext.SaveChanges();
        }

        [HttpPut("UpdatePosition")]
        public virtual int UpdatePosition(PositionInputDto PositionInputDto)
        {
            var Position = _mapper.Map<MbpPosition>(PositionInputDto);

            // 重新继承父级信息, todo优化 可以将此逻辑放到实体里面,当作领域逻辑
            var parentPosition = _defaultDbContext.MbpPositions.Where(m => m.Id == PositionInputDto.ParentId).FirstOrDefault();

            // 判断选择的上级是不是自己的下级岗位,这种选择是不合理的
            if (parentPosition.FullPositionName.StartsWith(Position.FullPositionName))
                throw new Exception("不能使用当前下级岗位作为父级岗位");

            // 刷新节点信息
            {
                // 刷新当前节点
                Position.SystemCode = parentPosition.SystemCode;
                Position.FullPositionName = string.Concat(parentPosition.FullPositionName, "/", Position.PositionName);
                Position.Level = parentPosition.Level + 1;
                Position.ParentPositionCode = parentPosition.PositionCode;
                Position.ParentPositionName = parentPosition.PositionName;

                _defaultDbContext.Attach(Position);

                // 刷新下级节点
                var current = _defaultDbContext.MbpPositions.Include("ChildrenPosition.ChildrenPosition.ChildrenPosition.ChildrenPosition.ChildrenPosition")
                .First(m => m.Id == Position.Id);
                RefreshChildrenInfo(Position, current.ChildrenPosition);

                _defaultDbContext.Attach(current);
            }

            _defaultDbContext.Update(Position);
            // 提交所有修改
            return _defaultDbContext.SaveChanges();
        }

        // 刷新部门信息
        private void RefreshChildrenInfo(MbpPosition current, List<MbpPosition> children)
        {
            children.ForEach(m =>
            {
                m.SystemCode = current.SystemCode;
                m.FullPositionName = string.Concat(current.FullPositionName, "/", m.PositionName);
                m.Level = current.Level + 1;
                m.ParentPositionCode = current.PositionCode;
                m.ParentPositionName = current.PositionName;

                RefreshChildrenInfo(m, m.ChildrenPosition);
            });
        }

        [HttpDelete("DeletePosition")]
        public virtual int DeletePosition(int PositionId)
        {
            var Position = _defaultDbContext.MbpPositions.Where(m => m.Id == PositionId).First();

            var Positions = _defaultDbContext.MbpPositions.Where(m => m.FullPositionName.StartsWith(Position.FullPositionName));

            _defaultDbContext.MbpPositions.RemoveRange(Positions);

            return _defaultDbContext.SaveChanges();
        }

        [HttpGet("GetPositions")]
        public virtual async Task<PagedList<PositionOutputDto>> GetPositions(SearchOptions<PositionSearchOptions> searchOptions)
        {
            int total = 0;

            List<MbpPosition> Positions = _defaultDbContext.MbpPositions.PageByAscending(searchOptions.PageSize, searchOptions.PageIndex, out total, (c) =>
            c.PositionName.Contains(searchOptions.Search.Name == null ? "" : searchOptions.Search.Name) &&
            c.PositionCode.Contains(searchOptions.Search.Code == null ? "" : searchOptions.Search.Code) &&
            searchOptions.Search.PositionType == 0 ? true : (c.PositionType == searchOptions.Search.PositionType) ||
            c.Id == 1,
            (c => c.Id)).ToList();

            var content = _mapper.Map<List<PositionOutputDto>>(Positions);

            var dictNodes = content.ToDictionary(x => x.Id);

            List<PositionOutputDto> result = new List<PositionOutputDto>();
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
            return new PagedList<PositionOutputDto>()
            {
                Content = result,
                PageIndex = searchOptions.PageIndex,
                PageSize = searchOptions.PageSize,
                Total = total
            };
        }

        [HttpGet("GetPosition")]
        public virtual PositionOutputDto GetPosition(int positionId)
        {
            var position = _defaultDbContext.MbpPositions.Where(p => p.Id == positionId).FirstOrDefault();
            var dto = _mapper.Map<PositionOutputDto>(position);
            return dto;
        }
    }
}
