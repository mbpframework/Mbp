using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Domain.DomainEntities.Train.Plan
{
    public class EmsTrainPlanMonth : EntityBase<int>, ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
