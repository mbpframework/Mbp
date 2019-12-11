using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Medical.Ai.Mbdp.Application.AccountService.Dto
{
    public class RoleOutputDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string SystemCode { get; set; }
    }
}
