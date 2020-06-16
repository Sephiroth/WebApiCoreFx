using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using MultipleCache.CoreComponent.Redis;

namespace MultipleCache.CoreComponent.Extension
{
    public static class MultipleCacheExtension
    {
        #region RedisCacheExtension
        public static IServiceCollection AddRedisCacheMiddleware(this IServiceCollection services,
            string redisConn, string instanceName = "SingletonRedis")
        {
            RedisCacheMiddleware.Options = new RedisCacheOptions
            {
                Configuration = redisConn,
                InstanceName = instanceName,
                ConfigurationOptions = StackExchange.Redis.ConfigurationOptions.Parse(redisConn)
            };
            return services.AddSingleton<RedisCacheMiddleware>();
        }

        public static IApplicationBuilder UserRedisCacheMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RedisCacheMiddleware>();
        }
        #endregion


    }
}