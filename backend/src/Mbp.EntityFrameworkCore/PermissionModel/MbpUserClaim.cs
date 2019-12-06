using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    /// <summary>
    /// 用户功能权限表,通过这个表,用户:功能权限点==N:N
    /// </summary>
    public class MbpUserClaim : EntityBase<int>, ISoftDelete
    {
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public MbpUser User { get; set; }

        public int MenuClaimId { get; set; }

        [ForeignKey("MenuClaimId")]
        public MbpMenuClaim MenuClaim { get; set; }

        public bool IsDeleted { get; set; }
    }
}
