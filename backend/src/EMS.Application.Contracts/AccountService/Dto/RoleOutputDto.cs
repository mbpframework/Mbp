using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Application.Contracts.AccountService.Dto
{
    public class RoleOutputDto : DtoBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string SystemCode { get; set; }
    }
}
