using AspectCore.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace AopDLL
{
    public class DothingBeforeInterceptorAttribute : AbstractInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            Console.WriteLine($"______DothingBeforeExecuting;next->{context.ServiceMethod.Name}");
            await next.Invoke(context);
        }
    }
}