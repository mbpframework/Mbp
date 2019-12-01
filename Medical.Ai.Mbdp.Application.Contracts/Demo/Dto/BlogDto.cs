using Medical.Ai.Mbdp.Domain.DomainEntities.Demo;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.Application.Contracts.Demo.Dto
{
    public class BlogDto
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public List<PostDto> Posts { get; set; } = new List<PostDto>();
    }
}
