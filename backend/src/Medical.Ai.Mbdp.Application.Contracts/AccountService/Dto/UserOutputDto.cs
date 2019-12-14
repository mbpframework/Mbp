using Mbp.EntityFrameworkCore.PermissionModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Medical.Ai.Mbdp.Application.Contracts.AccountService.Dto
{
    public class UserOutputDto
    {
        public int Id { get; set; }

        public string LoginName { get; set; }

        public string UserName { get; set; }

        public string Code { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public EnumUserStatus UserStatus { get; set; }

        public bool IsAdmin { get; set; }
    }
}
