using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Entity
{
    public interface IDeleteAudited
    {
        Guid? DeleterId { get; set; }

        DateTime? DeletionTime { get; set; }
    }
}
