using Mbp.Core.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.EntityFrameworkCore.PermissionModel
{
    public class MbpOperationLog : EntityBase<int>, ISoftDelete
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public string ClientIP { get; set; }

        public DateTime OpDateTime { get; set; }

        public string AppName { get; set; }

        public string ModuleName { get; set; }

        public string OpName { get; set; }

        public string Desc { get; set; }

        public bool IsDeleted { get; set; }
    }
}
