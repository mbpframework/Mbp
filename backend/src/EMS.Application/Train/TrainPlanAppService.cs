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
using Microsoft.EntityFrameworkCore;
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
    public class TrainPlanAppService : ITrainPlanAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        public TrainPlanAppService(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
        }

        [HttpPost("AddTrainPlanWeek")]
        public virtual int AddTrainPlanWeek(TrainPlanWeekInputDto trainPlanWeekInputDto)
        {
            var trainPlanWeek = _mapper.Map<EmsTrainPlanWeek>(trainPlanWeekInputDto);
            var dept = _defaultDbContext.MbpDepts.Where(d => d.Id == trainPlanWeekInputDto.DeptId).FirstOrDefault();
            if (dept != null) trainPlanWeek.DeptName = dept.DeptName;

            // 添加周一至周五的训练明细
            for (int i = 0; i < 5; i++)
            {
                EmsTrainPlanWeekDetail emsTrainPlanWeekDetail = new EmsTrainPlanWeekDetail();
                emsTrainPlanWeekDetail.DayOfWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), (i + 1).ToString());
                emsTrainPlanWeekDetail.TrainDate = trainPlanWeek.BeginTime.AddDays(i);

                trainPlanWeek.TrainPlanWeekDetails.Add(emsTrainPlanWeekDetail);
            }

            _defaultDbContext.EmsTrainPlanWeeks.Add(trainPlanWeek);

            return _defaultDbContext.SaveChanges();
        }

        [HttpPut("UpdateTrainPlanWeek")]
        public virtual int UpdateTrainPlanWeek(TrainPlanWeekInputDto trainPlanWeekInputDto)
        {
            var trainPlanWeek = _mapper.Map<EmsTrainPlanWeek>(trainPlanWeekInputDto);

            _defaultDbContext.Attach(trainPlanWeek);

            _defaultDbContext.EmsTrainPlanWeeks.Update(trainPlanWeek);

            return _defaultDbContext.SaveChanges();
        }

        [HttpDelete("DeleteTrainPlanWeek")]
        public virtual int DeleteTrainPlanWeek(int trainPlanWeekId)
        {
            var trainPlanWeek = _defaultDbContext.EmsTrainPlanWeeks.Where(r => r.Id == trainPlanWeekId).FirstOrDefault();
            _defaultDbContext.EmsTrainPlanWeeks.Remove(trainPlanWeek);

            return _defaultDbContext.SaveChanges();
        }

        [HttpGet("GetTrainPlanWeeks")]
        public virtual async Task<PagedList<TrainPlanWeekOutputDto>> GetTrainPlanWeeks(SearchOptions<TrainPlanWeekSearchOptions> searchOptions)
        {
            int total = 0;

            var subs = from trainPlanWeek in _defaultDbContext.EmsTrainPlanWeeks
                       join dept in _defaultDbContext.MbpDepts
                       on trainPlanWeek.DeptId equals dept.Id
                       where trainPlanWeek.Title.Contains(searchOptions.Search.Title == null ? "" : searchOptions.Search.Title) &&
           (!string.IsNullOrEmpty(searchOptions.Search.DeptName) ? dept.DeptName.Contains(searchOptions.Search.DeptName) : true) &&
           (searchOptions.Search.BeginTime == DateTime.MinValue ? true : trainPlanWeek.BeginTime >= searchOptions.Search.BeginTime) &&
           (searchOptions.Search.EndTime == DateTime.MaxValue ? true : trainPlanWeek.EndTime <= searchOptions.Search.EndTime)
                       select new TrainPlanWeekOutputDto
                       {
                           Id = trainPlanWeek.Id,
                           DeptId = dept.Id,
                           DeptName = dept.DeptName,
                           BeginTime = trainPlanWeek.BeginTime,
                           EndTime = trainPlanWeek.EndTime,
                           Title = trainPlanWeek.Title,
                           WeekNum = trainPlanWeek.WeekNum,
                           ConcurrencyStamp = trainPlanWeek.ConcurrencyStamp,
                           TrainPlanWeekDetails = new List<TrainPlanWeekDetailOutputDto>()
                       };

            // 分页
            var trainPlanWeeks = subs.PageByAscending(searchOptions.PageSize, searchOptions.PageIndex, out total, c => true, (c => c.Id)).ToList();

            // 返回列表分页数据
            return new PagedList<TrainPlanWeekOutputDto>()
            {
                Content = _mapper.Map<List<TrainPlanWeekOutputDto>>(trainPlanWeeks),
                PageIndex = searchOptions.PageIndex,
                PageSize = searchOptions.PageSize,
                Total = total
            };
        }

        [HttpGet("GetTrainPlanWeekDetails")]
        public virtual async Task<List<TrainPlanWeekDetailOutputDto>> GetTrainPlanWeekDetails(int trainPlanWeekId)
        {
            return _mapper.Map<List<TrainPlanWeekDetailOutputDto>>(_defaultDbContext.EmsTrainPlanWeekDetails.Where(wd => wd.EmsTrainPlanWeekId == trainPlanWeekId).ToList());
        }

        [HttpPut("UpdateTrainPlanWeekDetail")]
        public virtual int UpdateTrainPlanWeekDetail(TrainPlanWeekDetailInputDto trainPlanWeekDetailInputDto)
        {
            var trainPlanWeekDetail = _mapper.Map<EmsTrainPlanWeekDetail>(trainPlanWeekDetailInputDto);

            _defaultDbContext.Attach(trainPlanWeekDetail);

            _defaultDbContext.EmsTrainPlanWeekDetails.Update(trainPlanWeekDetail);

            return _defaultDbContext.SaveChanges();
        }
    }
}
