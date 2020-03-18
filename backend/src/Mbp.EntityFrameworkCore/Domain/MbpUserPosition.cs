using Mbp.Core.Entity;
using Mbp.EntityFrameworkCore.PermissionModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mbp.EntityFrameworkCore.Domain
{
    public class MbpUserPosition : EntityBase<int>, ISoftDelete
    {
        public int PositionId { get; set; }

        [ForeignKey("PositionId")]
        public MbpPosition Position { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public MbpUser User { get; set; }

        public bool IsDeleted { get; set; }
    }
}
