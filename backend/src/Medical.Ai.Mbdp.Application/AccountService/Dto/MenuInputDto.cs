using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Medical.Ai.Mbdp.Application.AccountService.Dto
{
    public class MenuInputDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public int Level { get; set; }

        [Required]
        public string Path { get; set; }

        public int ParentId { get; set; }
    }
}
