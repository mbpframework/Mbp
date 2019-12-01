using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Entity
{
    interface IModificationAudited
    {
        Guid? LastModifierId { get; set; }

        DateTime? LastModificationTime { get; set; }
    }
}
