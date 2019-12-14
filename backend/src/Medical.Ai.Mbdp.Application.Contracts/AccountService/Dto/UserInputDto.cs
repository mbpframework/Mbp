using Mbp.EntityFrameworkCore.PermissionModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Medical.Ai.Mbdp.Application.Contracts.AccountService.Dto
{
    public class UserInputDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string LoginName { get; set; }

        public string UserName { get; set; }

        public string Code { get; set; }

        [EmailAddress(ErrorMessage = "邮箱地址格式不正确")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        public EnumUserStatus UserStatus { get; set; }
    }
}
