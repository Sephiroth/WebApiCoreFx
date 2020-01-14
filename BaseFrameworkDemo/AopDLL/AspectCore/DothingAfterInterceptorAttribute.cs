using AspectCore.DynamicProxy;
using System;
using System.Threading.Tasks;

namespace AopDLL
{
    public class DothingAfterInterceptorAttribute : AbstractInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            await next.Invoke(context);
            Console.WriteLine($"______before->{context.ServiceMethod.Name}:Completed;DothingAfterExecuting");
        }
    }
}