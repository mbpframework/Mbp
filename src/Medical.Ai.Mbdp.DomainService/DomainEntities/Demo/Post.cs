using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.Domain.DomainEntities.Demo
{
    public class Post: EntityBase<int>, ISoftDelete
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsDeleted { get; set; }
    }
}
