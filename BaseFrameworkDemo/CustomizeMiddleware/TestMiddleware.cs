using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CustomizeMiddleware
{
    public class TestMiddleware //: IMiddleware
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
        public async Task Invoke(HttpContext context)
        {
            //context.Request.Headers.Add("TestMiddleware", new Microsoft.Extensions.Primitives.StringValues(DateTime.Now.ToString()));
            await _next.Invoke(context);
            //await context.Response.WriteAsync("(TestMiddleware的输出处理)");
        }

    }
}