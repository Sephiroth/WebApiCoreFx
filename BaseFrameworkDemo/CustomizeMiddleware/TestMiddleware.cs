using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CustomizeMiddleware
{
    /// <summary>
    /// 自定义中间件
    /// </summary>
    public class TestMiddleware
    {
        private readonly RequestDelegate _next;

        public TestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 给HTTP请求管道加入业务进去
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            //context.Request.Headers.Add("TestMiddleware", new Microsoft.Extensions.Primitives.StringValues(DateTime.Now.ToString()));
            await _next.Invoke(context);
            //await context.Response.WriteAsync("(TestMiddleware的输出处理)");
        }
    }

    /// <summary>
    /// 继承自IMiddleware的自定义中间件(必须在ConfigureServices中注册:services.AddSingleton<Test2Middleware>();)
    /// </summary>
    public class Test2Middleware : IMiddleware
    {
        public Test2Middleware() { }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            await next.Invoke(context);
        }
    }
}