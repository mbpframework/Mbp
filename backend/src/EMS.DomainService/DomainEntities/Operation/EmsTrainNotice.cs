using EMS.Domain.DomainEntities.Base;
using Mbp.Core.Entity;
using Mbp.Core.Entity.Aggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Domain.DomainEntities.Operation
{
    public class EmsTrainNotice : AggregateBase<int>, ISoftDelete, IHasAttachment
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string NoticeTitle { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string NoticeContent { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime PublishTime { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public EnumNoticeType NoticeType { get; set; }


        public EnumNoticeStatus NoticeStatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        public bool IsDeleted { get; set; }

        public Guid AttachmentRelative { get; set; }
    }
}
