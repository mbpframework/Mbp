using Mbp.Core.Entity;
using Mbp.Core.Entity.Aggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.EntityFrameworkCore.Domain
{
    public class MbpPosition : AggregateBase<int>, ISoftDelete
    {
        public string PositionName { get; set; }

        public string PositionCode { get; set; }

        public bool IsDeleted { get; set; }

        public string SystemCode { get; set; }
    }
}
