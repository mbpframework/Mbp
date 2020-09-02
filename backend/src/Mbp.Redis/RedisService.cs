using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mbp.Redis
{
    /*
 
 */
    /// <summary>
    /// Redis操作类，穿透，击穿，雪崩
    /// </summary>
    public class RedisService : IRedisService
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer = null;

        public RedisService(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        // 字符

        // 列表

        // 散列

        // 集合

        // 有序集合
    }
}
