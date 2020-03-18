using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Application.Contracts.AccountService.Dto
{
    public class RoleInputDto : DtoBase
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string SystemCode { get; set; }

        public byte[] ConcurrencyStamp { get; set; }
    }
}
