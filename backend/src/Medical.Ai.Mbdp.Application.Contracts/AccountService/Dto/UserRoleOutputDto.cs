using Mbp.Ddd.Application.Mbp.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.Application.Contracts.AccountService.Dto
{
    public class UserRoleOutputDto : DtoBase
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleCode { get; set; }

        public string SystemCode { get; set; }
    }
}
