using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Redis;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace MultipleCache.CoreComponent.Redis
{
    /// <summary>
    /// 自定义ActionFilter实现自动缓存
    /// </summary>
    public class MultipleCacheAttribute : ActionFilterAttribute
    {
        public static RedisCacheOptions RedisOptions { get; set; }
        public static MemoryCacheOptions MemOptions { get; set; }
        public static CacheTypeEnum CacheType { get; set; }

        private static readonly object lockObj = new object();
        private static IDistributedCache redisCache;
        private static Random random;
        private static IMemoryCache memCache;

        public MultipleCacheAttribute()
        {
            if (redisCache == null && RedisOptions != null)
            {
                lock (lockObj)
                {
                    redisCache ??= new RedisCache(RedisOptions);
                    random ??= new Random();
                }
            }
            if (memCache == null && MemOptions != null)
            {
                lock (lockObj)
                {
                    memCache ??= new MemoryCache(MemOptions);
                    random ??= new Random();
                }
            }
        }

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            #region 判断是否缓存,已缓存直接输出
            string key = await GetRequestKey(context.HttpContext.Request);
            object obj = await GetAsync(key);
            if (obj != null)
            {
                context.Result = new OkObjectResult(obj);
                return;
            }
            #endregion

            ActionExecutedContext executedContext = await next?.Invoke();

            #region 把结果缓存
            if (executedContext.Result is ObjectResult result)
            {
                await SetAsync(key, result.Value);
            }
            #endregion
        }

        /// <summary>
        /// 根据HttpRequest生成key
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<string> GetRequestKey(HttpRequest request)
        {
            string key = null;
            if (string.Equals(request.Method, "GET", System.StringComparison.OrdinalIgnoreCase))
            {
                key = $"{request.Path}_{request.QueryString}";
            }
            else if (string.Equals(request.Method, "POST", System.StringComparison.OrdinalIgnoreCase)
                || string.Equals(request.Method, "PUT", System.StringComparison.OrdinalIgnoreCase))
            {
                byte[] bodyCache = new byte[request.Body.Length];
                await request.Body.ReadAsync(bodyCache, 0, bodyCache.Length);
                key = $"{request.Path}_{System.Text.Encoding.UTF8.GetString(bodyCache)}";
            }
            return key;
        }

        private async Task<object> GetAsync(string key)
        {
            object data = null;
            if (Enum.Equals(CacheType, CacheTypeEnum.Redis))
            {
                data = await redisCache?.GetStringAsync(key);
            }
            else if (Enum.Equals(CacheType, CacheTypeEnum.Memory))
            {
                memCache.TryGetValue(key, out data);
            }
            return data;
        }

        private async Task SetAsync(string key, object obj)
        {
            int ranNum = random.Next(1000, 2000);
            if (Enum.Equals(CacheType, CacheTypeEnum.Redis))
            {
                await redisCache?.SetStringAsync(key, JsonConvert.SerializeObject(obj),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(ranNum)
                    });
            }
            else if (Enum.Equals(CacheType, CacheTypeEnum.Memory))
            {
                memCache?.Set(key, obj, DateTimeOffset.Now.AddSeconds(ranNum));
            }
        }

    }
}