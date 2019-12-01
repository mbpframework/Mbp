using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.CacheStorage
{
    public abstract class CacheStorage<TValue> : ICacheStorage<TValue> where TValue : class
    {
        public ConcurrentDictionary<string, TValue> KeyValuePairs { get; set; } = new ConcurrentDictionary<string, TValue>();

        public TValue GetValue(string key)
        {
            KeyValuePairs.TryGetValue(key, out TValue value);

            return value;
        }

        public bool SetValue(string key, TValue value)
        {
            return KeyValuePairs.TryAdd(key, value);
        }
    }
}
