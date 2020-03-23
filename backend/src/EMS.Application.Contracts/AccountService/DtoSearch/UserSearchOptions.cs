using Mbp.Ddd.Application.Mbp.Dto;
using Mbp.EntityFrameworkCore.Domain.Enums;
using Mbp.EntityFrameworkCore.PermissionModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EMS.Application.Contracts.AccountService.DtoSearch
{
    public class UserSearchOptions
    {
        public string UserName { get; set; } = string.Empty;

        public string LoginName { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string IsAdmin { get; set; } = string.Empty;

        public int DeptId { get; set; } = 0;

        public EnumUserSex UserSex { get; set; } = 0;

        public EnumPositionType PositionType { get; set; } = 0;

        public EnumUserType UserType { get; set; } = 0;
    }
}
