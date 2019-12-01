using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    public class MbpUser : EntityBase<int>, ISoftDelete
    {
        [MaxLength(256)]
        public string UserName { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        public string Password { get; set; }

        [MaxLength(256)]
        public string ConcurrencyStamp { get; set; }

        [MaxLength(256)]
        public string PhoneNumber { get; set; }

        public EnumUserStatus UserStatus { get; set; }

        public bool IsDeleted { get; set; }
    }
}
