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
    public class UserOutputDto : DtoBase
    {
        public int Id { get; set; }

        public string LoginName { get; set; }

        public string UserName { get; set; }

        public string Code { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public EnumUserStatus UserStatus { get; set; }

        public bool IsAdmin { get; set; }

        public string DeptName { get; set; }

        public UserDeptOutputDto UserDept { get; set; }

        //public int DeptId { get; set; }

        public EnumUserSex UserSex { get; set; }

        public EnumUserType UserType { get; set; }

        //public int PositionId { get; set; }
        public UserPositionOutputDto UserPosition { get; set; }

        public string PositionName { get; set; }

        public EnumPositionType PositionType { get; set; }

        public EnumUserEducation Education { get; set; }

        public string Major { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserAvatar { get; set; }

        public byte[] ConcurrencyStamp { get; set; }
    }
}
