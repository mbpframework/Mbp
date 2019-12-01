using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.Domain.DomainEntities.Demo
{
    public class Blog : EntityBase<int>, ISoftDelete
    {
        public string Url { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();

        public bool IsDeleted { get; set; }
    }
}
