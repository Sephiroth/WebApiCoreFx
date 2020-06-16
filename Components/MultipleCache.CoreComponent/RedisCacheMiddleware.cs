using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using System.Buffers;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MultipleCache.CoreComponent.Redis
{
    public class RedisCacheMiddleware : IMiddleware
    {
        public static RedisCacheOptions Options { get; set; }
        public static CacheTypeEnum CacheType { get; set; }
        private static object lockObj = new object();

        private static IDistributedCache redisCache;
        private ArrayPool<byte> pool;

        public RedisCacheMiddleware()
        {
            if (redisCache == null)
            {
                lock (lockObj)
                {
                    redisCache ??= new RedisCache(Options);
                    pool ??= ArrayPool<byte>.Shared;
                }
            }
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            string key = null;
            if (string.Equals(context.Request.Method, "GET", System.StringComparison.OrdinalIgnoreCase))
            {
                key = $"{context.Request.Path}_{context.Request.QueryString}";
                byte[] data = await redisCache.GetAsync(key);
                if (data != null)
                {
                    context.Response.Body = new MemoryStream(data);
                    return;
                }
            }
            else if (string.Equals(context.Request.Method, "POST", System.StringComparison.OrdinalIgnoreCase))
            {
                byte[] bodyCache = pool.Rent((int)context.Request.Body.Length);
                await context.Request.Body.ReadAsync(bodyCache, 0, bodyCache.Length);
                key = $"{context.Request.Path}_{Encoding.UTF8.GetString(bodyCache)}";
                pool.Return(bodyCache, true);
                byte[] data = await redisCache.GetAsync(key);
                if (data != null)
                {
                    context.Response.Body = new MemoryStream(data);
                    return;
                }
            }

            await next?.Invoke(context);
            byte[] buf = pool.Rent((int)context.Response.Body.Length);
            _ = await context.Response.Body.ReadAsync(buf, 0, buf.Length);
            await redisCache.SetAsync(key, buf);
            pool.Return(buf, true);
        }
    }
}