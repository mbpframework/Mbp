using AutoMapper;
using EMS.Application.Contracts.Operation;
using EMS.Application.Contracts.Operation.Dto;
using EMS.Application.Contracts.Operation.DtoSearch;
using EMS.Domain.DomainEntities.Operation;
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

namespace EMS.Application.Operation
{
    [Authorize]
    [AutoAop]
    [AutoWebApi]
    [Route("api/[controller]")]
    public class NoticeAppService : INoticeAppService
    {
        private readonly IMapper _mapper = AutofacService.Resolve<IMapper>();

        private readonly DefaultDbContext _defaultDbContext = null;

        public NoticeAppService(DefaultDbContext defaultDbContext)
        {
            _defaultDbContext = defaultDbContext;
        }

        [HttpPost("AddNotice")]
        public virtual int AddNotice(NoticeInputDto noticeInputDto)
        {
            var notice = _mapper.Map<EmsTrainNotice>(noticeInputDto);

            _defaultDbContext.EmsTrainNotices.Add(notice);

            return _defaultDbContext.SaveChanges();
        }

        [HttpPut("UpdateNotice")]
        public virtual int UpdateNotice(NoticeInputDto noticeInputDto)
        {
            var notice = _mapper.Map<EmsTrainNotice>(noticeInputDto);

            _defaultDbContext.Attach(notice);

            _defaultDbContext.EmsTrainNotices.Update(notice);

            return _defaultDbContext.SaveChanges();
        }

        [HttpDelete("DeleteNotice")]
        public virtual int DeleteNotice(int noticeId)
        {
            var notice = _defaultDbContext.EmsTrainNotices.Where(r => r.Id == noticeId).FirstOrDefault();
            _defaultDbContext.EmsTrainNotices.Remove(notice);

            return _defaultDbContext.SaveChanges();
        }

        [HttpPut("ChangeNoticeStatus")]
        public virtual int ChangeNoticeStatus(int noticeId, EnumNoticeStatus noticeStatus)
        {
            var notice = _defaultDbContext.EmsTrainNotices.Where(r => r.Id == noticeId).FirstOrDefault();

            _defaultDbContext.Attach(notice);
            notice.NoticeStatus = noticeStatus;

            _defaultDbContext.EmsTrainNotices.Update(notice);

            return _defaultDbContext.SaveChanges();
        }

        [HttpGet("GetNotices")]
        public virtual async Task<PagedList<NoticeOutputDto>> GetNotices(SearchOptions<NoticeSearchOptions> searchOptions)
        {
            int total = 0;

            var notices = _defaultDbContext.EmsTrainNotices.PageByAscending(searchOptions.PageSize, searchOptions.PageIndex, out total,
                (c) =>
            (!string.IsNullOrEmpty(searchOptions.Search.Title) ? c.NoticeTitle.Contains(searchOptions.Search.Title) : true) &&
           (searchOptions.Search.PublishTime != null ? c.PublishTime >= searchOptions.Search.PublishTime : true) &&
           (searchOptions.Search.NoticeType == 0 ? true : c.NoticeType == searchOptions.Search.NoticeType)&&
           (searchOptions.Search.NoticeStatus == 0 ? true : c.NoticeStatus == searchOptions.Search.NoticeStatus), (c => c.Id)).ToList();

            // 返回列表分页数据
            return new PagedList<NoticeOutputDto>()
            {
                Content = _mapper.Map<List<NoticeOutputDto>>(notices),
                PageIndex = searchOptions.PageIndex,
                PageSize = searchOptions.PageSize,
                Total = total
            };
        }
    }
}
