using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Entity
{
    interface ICreationAudited
    {
        DateTime CreationTime { get; set; }

        Guid? CreatorId { get; set; }
    }
}
