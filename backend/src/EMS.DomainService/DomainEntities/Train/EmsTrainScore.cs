using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Domain.DomainEntities.Train
{
    /// <summary>
    /// 训练成绩
    /// </summary>
    public class EmsTrainScore : EntityBase<int>, ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
