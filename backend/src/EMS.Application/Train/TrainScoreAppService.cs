using AutoMapper;
using EMS.Application.Contracts.Train;
using EMS.Application.Contracts.Train.Dto;
using EMS.Application.Contracts.Train.DtoSearch;
using EMS.Domain.DomainEntities.Train;
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
    public class TrainScoreAppService : ITrainScoreAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        public TrainScoreAppService(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
        }

        [HttpPost("AddTrainScore")]
        public virtual int AddTrainScore(TrainScoreInputDto trainScoreInputDto)
        {
            var trainScore = _mapper.Map<EmsTrainScore>(trainScoreInputDto);
            // 使用登录名来查找用户
            var user = _defaultDbContext.MbpUsers.Where(d => d.LoginName == trainScoreInputDto.LoginName).FirstOrDefault();
            if (user != null)
            {
                trainScore.UserName = user.UserName;
                trainScore.UserId = user.Id;
            }

            var subject = _defaultDbContext.EmsTrainSubjects.Where(s => s.Id == trainScoreInputDto.SubjectId).FirstOrDefault();
            if (subject != null)
            {
                trainScore.SubjectName = subject.SubjectName;
                trainScore.SubjectCode = subject.SubjectCode;
            }

            _defaultDbContext.EmsTrainScores.Add(trainScore);

            return _defaultDbContext.SaveChanges();
        }

        [HttpPut("UpdateTrainScore")]
        public virtual int UpdateTrainScore(TrainScoreInputDto trainScoreInputDto)
        {
            var trainScore = _mapper.Map<EmsTrainScore>(trainScoreInputDto);

            _defaultDbContext.Attach(trainScore);

            _defaultDbContext.EmsTrainScores.Update(trainScore);

            return _defaultDbContext.SaveChanges();
        }

        [HttpDelete("DeleteTrainScore")]
        public virtual int DeleteTrainScore(int trainScoreId)
        {
            var trainScore = _defaultDbContext.EmsTrainScores.Where(r => r.Id == trainScoreId).FirstOrDefault();
            _defaultDbContext.EmsTrainScores.Remove(trainScore);

            return _defaultDbContext.SaveChanges();
        }

        [HttpGet("GetTrainScores")]
        public virtual async Task<PagedList<TrainScoreOutputDto>> GetTrainScores(SearchOptions<TrainScoreSearchOptions> searchOptions)
        {
            int total = 0;

            var subs = from trainScore in _defaultDbContext.EmsTrainScores
                       where
            (!string.IsNullOrEmpty(searchOptions.Search.Major) ? trainScore.Major.Contains(searchOptions.Search.Major) : true) &&
           (!string.IsNullOrEmpty(searchOptions.Search.UserName) ? trainScore.UserName.Contains(searchOptions.Search.UserName) : true) &&
           (searchOptions.Search.TrainDate == DateTime.MinValue ? true : trainScore.TrainDate == searchOptions.Search.TrainDate) &&
           (searchOptions.Search.SubjectId == 0 ? true : trainScore.SubjectId == searchOptions.Search.SubjectId) &&
           (searchOptions.Search.Score == 0 ? true : trainScore.Score >= searchOptions.Search.Score)
                       select trainScore;

            // 分页
            var trainScores = subs.PageByAscending(searchOptions.PageSize, searchOptions.PageIndex, out total, c => true, (c => c.Id)).ToList();

            // 返回列表分页数据
            return new PagedList<TrainScoreOutputDto>()
            {
                Content = _mapper.Map<List<TrainScoreOutputDto>>(trainScores),
                PageIndex = searchOptions.PageIndex,
                PageSize = searchOptions.PageSize,
                Total = total
            };
        }
    }
}
