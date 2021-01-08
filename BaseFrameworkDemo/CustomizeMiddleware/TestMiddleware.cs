using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
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
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        /// <summary>
        /// 给HTTP请求管道加入业务进去
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            //context.Request.Headers.Add("TestMiddleware", new Microsoft.Extensions.Primitives.StringValues(DateTime.Now.ToString()));
            //bool tokenExist = context.Request.Headers.TryGetValue("Token", out StringValues values);
            //if (tokenExist)
            //{
            //    // todo: 验证token通过
            //    bool pass = false;
            //    if (pass)
            //    {
            await _next.Invoke(context);
            //    }
            //    // 验证失败:
            //    context.Response.StatusCode = 404;
            //}
            //else
            //{
            //    context.Response.StatusCode = 404;
            //}
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