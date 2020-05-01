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
    public class TrainPlanMonthAppService : ITrainPlanMonthAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        public TrainPlanMonthAppService(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
        }

        [HttpPost("AddTrainPlanMonth")]
        public virtual int AddTrainPlanMonth(TrainPlanMonthInputDto trainPlanMonthInputDto)
        {
            var trainPlanMonth = _mapper.Map<EmsTrainPlanMonth>(trainPlanMonthInputDto);
            var dept = _defaultDbContext.MbpDepts.Where(d => d.Id == trainPlanMonthInputDto.DeptId).FirstOrDefault();
            if (dept != null) trainPlanMonth.DeptName = dept.DeptName;

            _defaultDbContext.EmsTrainPlanMonths.Add(trainPlanMonth);

            return _defaultDbContext.SaveChanges();
        }

        [HttpPut("UpdateTrainPlanMonth")]
        public virtual int UpdateTrainPlanMonth(TrainPlanMonthInputDto trainPlanMonthInputDto)
        {
            var trainPlanMonth = _mapper.Map<EmsTrainPlanMonth>(trainPlanMonthInputDto);

            _defaultDbContext.Attach(trainPlanMonth);

            _defaultDbContext.EmsTrainPlanMonths.Update(trainPlanMonth);

            return _defaultDbContext.SaveChanges();
        }

        [HttpDelete("DeleteTrainPlanMonth")]
        public virtual int DeleteTrainPlanMonth(int trainPlanMonthId)
        {
            var trainPlanMonth = _defaultDbContext.EmsTrainPlanMonths.Where(r => r.Id == trainPlanMonthId).FirstOrDefault();
            _defaultDbContext.EmsTrainPlanMonths.Remove(trainPlanMonth);

            return _defaultDbContext.SaveChanges();
        }

        [HttpGet("GetTrainPlanMonths")]
        public virtual async Task<PagedList<TrainPlanMonthOutputDto>> GetTrainPlanMonths(SearchOptions<TrainPlanMonthSearchOptions> searchOptions)
        {
            int total = 0;

            var subs = from trainPlanMonth in _defaultDbContext.EmsTrainPlanMonths
                       join dept in _defaultDbContext.MbpDepts
                       on trainPlanMonth.DeptId equals dept.Id
                       join attachment in _defaultDbContext.EmsAttachments
                       on trainPlanMonth.AttachmentRelative equals attachment.BussinessId
                       into attachments from attachment in attachments.DefaultIfEmpty()
                       where trainPlanMonth.Title.Contains(searchOptions.Search.Title == null ? "" : searchOptions.Search.Title) &&
           (!string.IsNullOrEmpty(searchOptions.Search.DeptName) ? dept.DeptName.Contains(searchOptions.Search.DeptName) : true) &&
           (!string.IsNullOrEmpty(searchOptions.Search.Month) ? trainPlanMonth.Month == searchOptions.Search.Month : true)
                       select new TrainPlanMonthOutputDto
                       {
                           Id = trainPlanMonth.Id,
                           DeptId = dept.Id,
                           DeptName = dept.DeptName,
                           Month = trainPlanMonth.Month,
                           Title = trainPlanMonth.Title,
                           AttachmentRelative = trainPlanMonth.AttachmentRelative,
                           Remark = trainPlanMonth.Remark,
                           ConcurrencyStamp = trainPlanMonth.ConcurrencyStamp,
                           AttachementUrl = attachment.Url,
                           AttachmentName = attachment.Name
                       };

            // 分页
            var trainPlanMonths = subs.PageByAscending(searchOptions.PageSize, searchOptions.PageIndex, out total, c => true, (c => c.Id)).ToList();

            // 返回列表分页数据
            return new PagedList<TrainPlanMonthOutputDto>()
            {
                Content = _mapper.Map<List<TrainPlanMonthOutputDto>>(trainPlanMonths),
                PageIndex = searchOptions.PageIndex,
                PageSize = searchOptions.PageSize,
                Total = total
            };
        }
    }
}
