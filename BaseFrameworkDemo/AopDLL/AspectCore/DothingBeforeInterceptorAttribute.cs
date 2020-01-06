using AspectCore.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace AopDLL
{
    public class DothingBeforeInterceptorAttribute : AbstractInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            Console.WriteLine($"DothingBeforeExecuting;next->{next.Method.Name}");
            await next.Invoke(context);
        }
    }
}