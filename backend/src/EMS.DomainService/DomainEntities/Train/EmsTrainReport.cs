using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Domain.DomainEntities.Train
{
    /// <summary>
    /// 训练报告
    /// </summary>
    public class EmsTrainReport : EntityBase<int>, ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
