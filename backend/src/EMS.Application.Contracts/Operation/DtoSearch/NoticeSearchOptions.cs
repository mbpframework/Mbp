using EMS.Domain.DomainEntities.Base;
using EMS.Domain.DomainEntities.Operation;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.Operation.DtoSearch
{
    public class NoticeSearchOptions
    {
        public string Title { get; set; }

        public DateTime PublishTime { get; set; }

        public EnumNoticeType NoticeType { get; set; }

        public EnumNoticeStatus NoticeStatus { get; set; }
    }
}
