using Mbp.Ddd.Application.Mbp.Dto;
using Mbp.EntityFrameworkCore.Domain.Enums;
using Mbp.EntityFrameworkCore.PermissionModel;
using Mbp.EntityFrameworkCore.PermissionModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EMS.Application.Contracts.AccountService.Dto
{
    public class UserInputDto : DtoBase
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

        [Required]
        public int DeptId { get; set; }

        [Required]
        public EnumUserSex UserSex { get; set; }

        [Required]
        public EnumUserType UserType { get; set; }

        [Required]
        public EnumPositionType PositionType { get; set; }

        [Required]
        public EnumUserEducation Education { get; set; }

        [Required]
        public string Major { get; set; }

        public EnumUserStatus UserStatus { get; set; }

        public byte[] ConcurrencyStamp { get; set; }
    }
}
