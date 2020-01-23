using Mbp.Core.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Mbp.Core.Entity.Aggregate
{
    public class AggregateBase<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TKey Id { get; set; }

        [Timestamp]
        [ConcurrencyCheck]
        public byte[] ConcurrencyStamp { get; set; }
    }
}
