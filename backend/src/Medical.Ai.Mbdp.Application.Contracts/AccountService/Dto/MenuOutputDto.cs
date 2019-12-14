using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.Application.Contracts.AccountService.Dto
{
    public class MenuOutputDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public int Level { get; set; }

        public string Path { get; set; }

        public int ParentId { get; set; }

        public int Order { get; set; }
    }
}
