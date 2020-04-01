using AutoMapper;
using EMS.Application.Contracts.Base;
using EMS.Application.Contracts.Base.Dto;
using EMS.Application.Contracts.Base.DtoSearch;
using EMS.Domain.DomainEntities.Base;
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

namespace EMS.Application.Base
{
    [Authorize(Roles = "admin")]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class TrainSubjectAppService : ITrainSubjectAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        public TrainSubjectAppService(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
        }

        [HttpPost("AddSubject")]
        public virtual int AddSubject(TrainSubjectInputDto trainSubjectInputDto)
        {
            var subject = _mapper.Map<EmsTrainSubject>(trainSubjectInputDto);

            _defaultDbContext.EmsTrainSubjects.Add(subject);

            return _defaultDbContext.SaveChanges();
        }

        [HttpPut("UpdateSubject")]
        public virtual int UpdateSubject(TrainSubjectInputDto trainSubjectInputDto)
        {
            var subject = _mapper.Map<EmsTrainSubject>(trainSubjectInputDto);

            _defaultDbContext.Attach(subject);

            _defaultDbContext.EmsTrainSubjects.Update(subject);

            return _defaultDbContext.SaveChanges();
        }

        [HttpDelete("DeleteSubject")]
        public virtual int DeleteSubject(int subjectId)
        {
            var subject = _defaultDbContext.EmsTrainSubjects.Where(r => r.Id == subjectId).FirstOrDefault();
            _defaultDbContext.EmsTrainSubjects.Remove(subject);

            return _defaultDbContext.SaveChanges();
        }

        [HttpGet("GetSubjects")]
        public virtual async Task<PagedList<TrainSubjectOutputDto>> GetSubjects(SearchOptions<TrainSubjectSearchOptions> searchOptions)
        {
            int total = 0;

            var subs = from subject in _defaultDbContext.EmsTrainSubjects
                       join position in _defaultDbContext.MbpPositions
                       on subject.PositionId equals position.Id
                       where subject.SubjectName.Contains(searchOptions.Search.Name == null ? "" : searchOptions.Search.Name) &&
           (!string.IsNullOrEmpty(searchOptions.Search.Code) ? subject.SubjectCode == searchOptions.Search.Code : true) &&
           (searchOptions.Search.PositionId == 0 ? true : subject.PositionId == searchOptions.Search.PositionId)
                       select new TrainSubjectOutputDto
                       {
                           Id = subject.Id,
                           PositionId = subject.PositionId,
                           ConcurrencyStamp = subject.ConcurrencyStamp,
                           PositionName = position.PositionName,
                           SubjectCode = subject.SubjectCode,
                           SubjectName = subject.SubjectName
                       };
            // 分页
            var subjects = subs.PageByAscending(searchOptions.PageSize, searchOptions.PageIndex, out total, c => true, (c => c.Id)).ToList();

            // 返回列表分页数据
            return new PagedList<TrainSubjectOutputDto>()
            {
                Content = _mapper.Map<List<TrainSubjectOutputDto>>(subjects),
                PageIndex = searchOptions.PageIndex,
                PageSize = searchOptions.PageSize,
                Total = total
            };
        }
    }
}
