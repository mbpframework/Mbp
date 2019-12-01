using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    public class MbpRole : EntityBase<int>, ISoftDelete
    {
        [MaxLength(256)]
        public string Name { get; set; }

        public string ConcurrencyStamp { get; set; }

        public bool IsDeleted { get; set; }
    }
}
