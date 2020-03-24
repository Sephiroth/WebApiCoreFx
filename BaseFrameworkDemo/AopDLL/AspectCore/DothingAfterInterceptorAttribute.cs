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
            //Console.WriteLine($"______before->{context.ServiceMethod.Name}:Completed;DothingAfterExecuting");
            if (context.ProxyMethod.Name.Contains("GetAsync"))
            {
                if (context.ReturnValue is Task<System.Collections.Generic.List<DBModel.Entity.TbUser>>)
                {
                    var rtnVal = context.ReturnValue as Task<System.Collections.Generic.List<DBModel.Entity.TbUser>>;
                    rtnVal.Result.Add(new DBModel.Entity.TbUser() { Name = "Test" });
                }
            }
        }
    }
}