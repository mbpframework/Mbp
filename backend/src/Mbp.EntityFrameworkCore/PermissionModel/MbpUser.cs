using Mbp.Core.Entity;
using Mbp.Core.Entity.Aggregate;
using Mbp.EntityFrameworkCore.Domain;
using Mbp.EntityFrameworkCore.Domain.Enums;
using Mbp.EntityFrameworkCore.PermissionModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    /// <summary>
    /// 用户表,一个用户有多个用户角色关系
    /// </summary>
    public class MbpUser : AggregateBase<int>, ISoftDelete
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
        public string Code { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        public string Password { get; set; }

        [MaxLength(256)]
        public string PhoneNumber { get; set; }

        public EnumUserStatus UserStatus { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public EnumUserSex UserSex { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        public EnumUserEducation Education { get; set; }

        /// <summary>
        /// 专业
        /// </summary>
        public string Major { get; set; }

        /// <summary>
        /// 人员类别
        /// </summary>
        public EnumUserType UserType { get; set; }

        /// <summary>
        /// 岗位类别
        /// </summary>
        public EnumPositionType PositionType { get; set; }

        /// <summary>
        /// 用户岗位,用户和岗位 m:n,一个用户一个岗位
        /// </summary>
        public MbpUserPosition UserPosition { get; set; }

        /// <summary>
        /// 一个用户,有多个角色
        /// </summary>
        public List<MbpUserRole> UserRoles { get; set; }

        /// <summary>
        /// 一个用户一个部门,用户和部门 m:n
        /// </summary>
        public MbpUserDept UserDept { get; set; }

        /// <summary>
        /// 是不是超级管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        public bool IsDeleted { get; set; }

        public string SystemCode { get; set; }
    }
}
