using StackExchange.Redis;
using System;

namespace MultipleCache.CoreComponent
{
    public class RedisInstance
    {
        private static readonly object _lockObj = new object();
        private RedisInstance() { }

        public static ConnectionMultiplexer Multiplexer { get; private set; }

        public void InitMultiplexer(string connStrs)
        {
            if (string.IsNullOrEmpty(connStrs?.Trim())) { throw new ArgumentNullException(nameof(connStrs)); }
            if (Multiplexer == null)
            {
                lock (_lockObj)
                {
                    Multiplexer ??= ConnectionMultiplexer.ConnectAsync(connStrs).GetAwaiter().GetResult();
                }
            }
        }

    }
}