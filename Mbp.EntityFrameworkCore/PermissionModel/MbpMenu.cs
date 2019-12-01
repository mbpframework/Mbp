using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    public class MbpMenu : EntityBase<int>, ISoftDelete
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string Code { get; set; }

        public int PraentId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
