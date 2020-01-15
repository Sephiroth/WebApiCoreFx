using Microsoft.AspNetCore.Builder;

namespace CustomizeMiddleware
{
    public static class CustomizeMiddlewareEntension
    {
        /// <summary>
        /// 使用自定义的TestMiddleware
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseTestMiddleware(this IApplicationBuilder builder)
        {
            // 使用UseMiddleware将自定义中间件添加到请求处理管道中
            return builder.UseMiddleware<TestMiddleware>();
        }

        /// <summary>
        /// 使用自定义的Test2Middleware
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseTest2Middleware(this IApplicationBuilder builder)
        {
            // 使用UseMiddleware将自定义中间件添加到请求处理管道中
            return builder.UseMiddleware<Test2Middleware>();
        }
    }
}