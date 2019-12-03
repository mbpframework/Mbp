using System;
using System.Collections.Generic;
using System.Text;

namespace Medical.Ai.Mbdp.Application.AccountService.Dto
{
    public class UserRoleOutputDto
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleCode { get; set; }
    }
}
