using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Domain.DomainEntities.Train
{
    /// <summary>
    /// 训练登记
    /// </summary>
    public class EmsTrainRecord : EntityBase<int>, ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
