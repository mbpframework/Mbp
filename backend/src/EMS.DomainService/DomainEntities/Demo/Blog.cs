using Mbp.Core.Entity;
using Mbp.Core.Entity.Aggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Domain.DomainEntities.Demo
{
    public class Blog : AggregateBase<int>, ISoftDelete
    {
        public string Url { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();

        public bool IsDeleted { get; set; }
    }
}
