using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    /// <summary>
    /// 用户角色表,也可以叫角色用户表,通过这个表,角色:用户==N:N
    /// </summary>
    public class MbpUserRole : EntityBase<int>
    {
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public MbpUser User { get; set; }

        [ForeignKey("RoleId")]
        public MbpRole Role { get; set; }

        public int RoleId { get; set; }
    }
}
