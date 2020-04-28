using Mbp.Core.Entity;
using Mbp.Core.Entity.Aggregate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Domain.DomainEntities.Base
{
    public class EmsAttachment : EntityBase<int>, ISoftDelete
    {
        /// <summary>
        /// 附件名称
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// 附件路径
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string Url { get; set; }


        /// <summary>
        /// 附件类型(图片：image,文档:file)
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string AttachmentTypeElementCode { get; set; }


        /// <summary>
        /// 业务类型
        /// </summary>
        [Required]
        [MaxLength(256)]
        public string BussinessTypeElementCode { get; set; }


        /// <summary>
        /// 业务Id
        /// </summary>
        public Guid BussinessId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
