using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    public class MbpUserDept : EntityBase<int>, ISoftDelete
    {
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public MbpUser User { get; set; }

        [ForeignKey("DeptId")]
        public MbpDept Dept { get; set; }

        public int DeptId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
