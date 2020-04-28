using EMS.Domain.DomainEntities.Operation;
using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.Operation.Dto
{
    public class NoticeOutputDto : DtoBase
    {
        public int Id { get; set; }

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
        /// 附件
        /// </summary>
        public Guid AttachmentRelative { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public byte[] ConcurrencyStamp { get; set; }
    }
}
