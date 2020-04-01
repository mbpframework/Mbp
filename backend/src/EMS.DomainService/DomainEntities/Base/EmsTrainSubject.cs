using Mbp.Core.Entity;
using Mbp.Core.Entity.Aggregate;
using Mbp.EntityFrameworkCore.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EMS.Domain.DomainEntities.Base
{
    public class EmsTrainSubject : AggregateBase<int>, ISoftDelete
    {
        /// <summary>
        /// 岗位Id
        /// </summary>
        public int PositionId { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 科目编码
        /// </summary>
        public string SubjectCode { get; set; }

        public bool IsDeleted { get; set; }
    }
}
