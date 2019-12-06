using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    /// <summary>
    /// 菜单操作表
    /// </summary>
    public class MbpMenuClaim : EntityBase<int>, ISoftDelete
    {
        public int MenuId { get; set; }

        [ForeignKey("MenuId")]
        public MbpMenu Menu { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }

        public bool IsDeleted { get; set; }
    }
}
