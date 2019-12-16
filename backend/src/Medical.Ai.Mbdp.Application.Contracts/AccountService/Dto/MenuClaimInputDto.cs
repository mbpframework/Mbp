using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Medical.Ai.Mbdp.Application.Contracts.AccountService.Dto
{
    public class MenuClaimInputDto : DtoBase
    {
        [Required]
        public int MenuId { get; set; }

        [Required]
        public string ClaimType { get; set; }

        [Required]
        public string ClaimValue { get; set; }
    }
}
