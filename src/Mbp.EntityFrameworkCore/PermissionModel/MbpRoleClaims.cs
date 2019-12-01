using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    public class MbpRoleClaims : EntityBase<int>
    {
        public int RoleId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}
