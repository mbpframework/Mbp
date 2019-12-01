using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Core.CacheStorage
{
    public interface ICacheStorage<TValue> where TValue : class
    {
        TValue GetValue(string key);

        bool SetValue(string key, TValue value);
    }
}
