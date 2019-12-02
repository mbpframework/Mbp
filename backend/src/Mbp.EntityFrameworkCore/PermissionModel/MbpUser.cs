using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    /// <summary>
    /// 用户表,一个用户有多个用户角色关系
    /// </summary>
    public class MbpUser : EntityBase<int>, ISoftDelete
    {
        /// <summary>
        /// 用作登录的名称
        /// </summary>
        [MaxLength(256)]

        public string LoginName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        [MaxLength(256)]
        public string UserName { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        public string Password { get; set; }

        [MaxLength(256)]
        public string ConcurrencyStamp { get; set; }

        [MaxLength(256)]
        public string PhoneNumber { get; set; }

        public EnumUserStatus UserStatus { get; set; }

        /// <summary>
        /// 一个用户,有多个角色
        /// </summary>
        public List<MbpUserRole> UserRoles { get; set; }

        /// <summary>
        /// 是不是超级管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        public bool IsDeleted { get; set; }
    }
}
