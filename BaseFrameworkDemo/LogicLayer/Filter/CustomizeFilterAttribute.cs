using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace LogicLayer.Filter
{
    public sealed class CustomizeFilterAttribute : ActionFilterAttribute
    {
        public CustomizeFilterAttribute() { }

        public CustomizeFilterAttribute(int order)
        {
            Order = order;
        }

        #region 
        // 在Action执行之后由 MVC 框架调用
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Console.WriteLine("___OnActionExecuted");
        }

        // 在Action执行之前由 MVC 框架调用
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("___OnActionExecuting");
        }

        // 在执行Result后由 MVC 框架调用
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine("___OnResultExecuted");
        }
        // 在执行Result之前由 MVC 框架调用
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine("___OnResultExecuting");
        }

        #endregion

        #region 异步Action优先,与以上4个方法不能同时生效,
        //
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("___OnActionExecutionAsync_before");
            await next();
            Console.WriteLine("___OnActionExecutionAsync_after");
        }

        // 
        public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            Console.WriteLine("___OnResultExecutionAsync_before");
            await next();
            Console.WriteLine("___OnResultExecutionAsync_after");
        }
        #endregion

    }
}