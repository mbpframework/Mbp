using AutoMapper;
using EMS.Application.Contracts.Train;
using EMS.Application.Contracts.Train.Dto;
using EMS.Application.Contracts.Train.DtoSearch;
using EMS.Domain.DomainEntities.Train.Plan;
using EMS.EntityFrameworkCore.EntityFrameworkCore;
using Mbp.AspNetCore.Mvc.Convention;
using Mbp.Core.Core;
using Mbp.Core.Modularity;
using Mbp.Ddd.Application.Mbp.UI;
using Mbp.Ddd.Application.System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Application.Train
{
    [Authorize]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class TrainPlanQuarterAppService : ITrainPlanMonthAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        public TrainPlanQuarterAppService(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
        }

        [HttpPost("AddTrainPlanQuarter")]
        public virtual int AddTrainPlanQuarter(TrainPlanQuarterInputDto trainPlanQuarterInputDto)
        {
            var trainPlanQuarter = _mapper.Map<EmsTrainPlanQuarter>(trainPlanQuarterInputDto);
            var dept = _defaultDbContext.MbpDepts.Where(d => d.Id == trainPlanQuarterInputDto.DeptId).FirstOrDefault();
            if (dept != null) trainPlanQuarter.DeptName = dept.DeptName;

            _defaultDbContext.EmsTrainPlanQuarters.Add(trainPlanQuarter);

            return _defaultDbContext.SaveChanges();
        }

        [HttpPut("UpdateTrainPlanQuarter")]
        public virtual int UpdateTrainPlanQuarter(TrainPlanQuarterInputDto trainPlanQuarterInputDto)
        {
            var trainPlanQuarter = _mapper.Map<EmsTrainPlanQuarter>(trainPlanQuarterInputDto);

            _defaultDbContext.Attach(trainPlanQuarter);

            _defaultDbContext.EmsTrainPlanQuarters.Update(trainPlanQuarter);

            return _defaultDbContext.SaveChanges();
        }

        [HttpDelete("DeleteTrainPlanQuarter")]
        public virtual int DeleteTrainPlanQuarter(int trainPlanQuarterId)
        {
            var trainPlanQuarter = _defaultDbContext.EmsTrainPlanQuarters.Where(r => r.Id == trainPlanQuarterId).FirstOrDefault();
            _defaultDbContext.EmsTrainPlanQuarters.Remove(trainPlanQuarter);

            return _defaultDbContext.SaveChanges();
        }

        [HttpGet("GetTrainPlanQuarters")]
        public virtual async Task<PagedList<TrainPlanQuarterOutputDto>> GetTrainPlanQuarters(SearchOptions<TrainPlanQuarterSearchOptions> searchOptions)
        {
            int total = 0;

            var subs = from trainPlanQuarter in _defaultDbContext.EmsTrainPlanQuarters
                       join dept in _defaultDbContext.MbpDepts
                       on trainPlanQuarter.DeptId equals dept.Id
                       where trainPlanQuarter.Title.Contains(searchOptions.Search.Title == null ? "" : searchOptions.Search.Title) &&
           (!string.IsNullOrEmpty(searchOptions.Search.DeptName) ? dept.DeptName.Contains(searchOptions.Search.DeptName) : true) &&
           (searchOptions.Search.Quarter > 0 ? trainPlanQuarter.Quarter == searchOptions.Search.Quarter : true)
                       select new TrainPlanQuarterOutputDto
                       {
                           Id = trainPlanQuarter.Id,
                           DeptId = dept.Id,
                           DeptName = dept.DeptName,
                           Quarter = trainPlanQuarter.Quarter,
                           Title = trainPlanQuarter.Title,
                           AttachmentRelative = trainPlanQuarter.AttachmentRelative,
                           Remark = trainPlanQuarter.Remark,
                           ConcurrencyStamp = trainPlanQuarter.ConcurrencyStamp
                       };

            // 分页
            var trainPlanQuarters = subs.PageByAscending(searchOptions.PageSize, searchOptions.PageIndex, out total, c => true, (c => c.Id)).ToList();

            // 返回列表分页数据
            return new PagedList<TrainPlanQuarterOutputDto>()
            {
                Content = _mapper.Map<List<TrainPlanQuarterOutputDto>>(trainPlanQuarters),
                PageIndex = searchOptions.PageIndex,
                PageSize = searchOptions.PageSize,
                Total = total
            };
        }
    }
}
