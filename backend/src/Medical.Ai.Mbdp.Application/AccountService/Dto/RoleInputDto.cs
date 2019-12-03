using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Medical.Ai.Mbdp.Application.AccountService.Dto
{
    public class RoleInputDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
