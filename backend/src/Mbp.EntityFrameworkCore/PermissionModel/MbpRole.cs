using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    /// <summary>
    /// 角色表,一个角色可以有多个用户角色关系,一个角色拥有多个功能
    /// </summary>
    public class MbpRole : EntityBase<int>, ISoftDelete
    {
        [MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Code { get; set; }

        public string ConcurrencyStamp { get; set; }

        public List<MbpRoleMenu> RoleMenus { get; set; }

        public List<MbpUserRole> UserRoles { get; set; }

        public bool IsDeleted { get; set; }
    }
}
