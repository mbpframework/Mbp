using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    public class MbpUserRole : EntityBase<int>
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }
    }
}
