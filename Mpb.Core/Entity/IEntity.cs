using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.Entity
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
    }
}
