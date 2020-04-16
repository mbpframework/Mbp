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
        /// 科目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>
        /// 科目编码
        /// </summary>
        public string SubjectCode { get; set; }

        /// <summary>
        /// 训练类型
        /// </summary>
        public EnumTrainType TrainType { get; set; }

        /// <summary>
        /// 训练课时(小时)
        /// </summary>
        public decimal TrainHour { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public bool IsDeleted { get; set; }
    }
}
