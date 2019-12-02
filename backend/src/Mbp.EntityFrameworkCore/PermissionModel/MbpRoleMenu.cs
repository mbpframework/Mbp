using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    /// <summary>
    /// 角色菜单关系表,通过此表 角色:菜单==N:N
    /// </summary>
    public class MbpRoleMenu : EntityBase<int>
    {
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public MbpRole Role { get; set; }

        public int MenuId { get; set; }

        [ForeignKey("MenuId")]
        public MbpMenu Menu { get; set; }
    }
}
