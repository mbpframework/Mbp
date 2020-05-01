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
    public class TrainPlanYearAppService : ITrainPlanMonthAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        public TrainPlanYearAppService(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
        }

        [HttpPost("AddTrainPlanYear")]
        public virtual int AddTrainPlanYear(TrainPlanYearInputDto trainPlanYearInputDto)
        {
            var trainPlanYear = _mapper.Map<EmsTrainPlanYear>(trainPlanYearInputDto);
            var dept = _defaultDbContext.MbpDepts.Where(d => d.Id == trainPlanYearInputDto.DeptId).FirstOrDefault();
            if (dept != null) trainPlanYear.DeptName = dept.DeptName;

            _defaultDbContext.EmsTrainPlanYears.Add(trainPlanYear);

            return _defaultDbContext.SaveChanges();
        }

        [HttpPut("UpdateTrainPlanYear")]
        public virtual int UpdateTrainPlanYear(TrainPlanYearInputDto trainPlanYearInputDto)
        {
            var trainPlanYear = _mapper.Map<EmsTrainPlanYear>(trainPlanYearInputDto);

            _defaultDbContext.Attach(trainPlanYear);

            _defaultDbContext.EmsTrainPlanYears.Update(trainPlanYear);

            return _defaultDbContext.SaveChanges();
        }

        [HttpDelete("DeleteTrainPlanYear")]
        public virtual int DeleteTrainPlanYear(int trainPlanYearId)
        {
            var trainPlanYear = _defaultDbContext.EmsTrainPlanYears.Where(r => r.Id == trainPlanYearId).FirstOrDefault();
            _defaultDbContext.EmsTrainPlanYears.Remove(trainPlanYear);

            return _defaultDbContext.SaveChanges();
        }

        [HttpGet("GetTrainPlanYears")]
        public virtual async Task<PagedList<TrainPlanYearOutputDto>> GetTrainPlanYears(SearchOptions<TrainPlanYearSearchOptions> searchOptions)
        {
            int total = 0;

            var subs = from trainPlanYear in _defaultDbContext.EmsTrainPlanYears
                       join dept in _defaultDbContext.MbpDepts
                       on trainPlanYear.DeptId equals dept.Id
                       where trainPlanYear.Title.Contains(searchOptions.Search.Title == null ? "" : searchOptions.Search.Title) &&
           (!string.IsNullOrEmpty(searchOptions.Search.DeptName) ? dept.DeptName.Contains(searchOptions.Search.DeptName) : true) &&
           (searchOptions.Search.Year > 0 ? trainPlanYear.Year == searchOptions.Search.Year : true)
                       select new TrainPlanYearOutputDto
                       {
                           Id = trainPlanYear.Id,
                           DeptId = dept.Id,
                           DeptName = dept.DeptName,
                           Year = trainPlanYear.Year,
                           Title = trainPlanYear.Title,
                           AttachmentRelative = trainPlanYear.AttachmentRelative,
                           Remark = trainPlanYear.Remark,
                           ConcurrencyStamp = trainPlanYear.ConcurrencyStamp
                       };

            // 分页
            var trainPlanYears = subs.PageByAscending(searchOptions.PageSize, searchOptions.PageIndex, out total, c => true, (c => c.Id)).ToList();

            // 返回列表分页数据
            return new PagedList<TrainPlanYearOutputDto>()
            {
                Content = _mapper.Map<List<TrainPlanYearOutputDto>>(trainPlanYears),
                PageIndex = searchOptions.PageIndex,
                PageSize = searchOptions.PageSize,
                Total = total
            };
        }
    }
}
