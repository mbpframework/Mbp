using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    public class MbpRoleMenu : EntityBase<int>
    {
        public int RoleId { get; set; }

        public int MenuId { get; set; }
    }
}
