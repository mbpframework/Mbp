using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    public class MbpUserClaims : EntityBase<int>
    {
        public int UserId { get; set; }

        public string ClaimType { get; set; }

        public string ClaimValue { get; set; }
    }
}
