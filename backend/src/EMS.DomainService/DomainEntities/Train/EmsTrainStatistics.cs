using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Domain.DomainEntities.Train
{
    /// <summary>
    /// 训练统计
    /// </summary>
    public class EmsTrainStatistics : EntityBase<int>, ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
