using ServiceStack.Redis;

namespace DBLayer.Redis
{
    public class RedisUtil
    {
        public RedisUtil() { }

        public PooledRedisClientManager CreateRedis(string[] readWriteHosts, string[] readOnlyHosts, string pwd, long db = 0)
        {
            PooledRedisClientManager client = new PooledRedisClientManager(readWriteHosts, readOnlyHosts, new RedisClientManagerConfig
            {
                MaxWritePoolSize = 10000, // “写”链接池链接数 
                MaxReadPoolSize = 100, // “读”链接池链接数 
                AutoStart = true,
                DefaultDb = db
            });
            return client;
        }


    }
}