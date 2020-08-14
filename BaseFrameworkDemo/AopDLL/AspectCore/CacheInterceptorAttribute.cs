using AspectCore.DynamicProxy;
using AspectCore.DynamicProxy.Parameters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AopDLL.AspectCore
{
    public class CacheInterceptorAttribute : AbstractInterceptorAttribute
    {
        public CacheInterceptorAttribute(object cacheObj)
        {
            _cacheType = cacheObj.GetType();
        }

        private Type _cacheType;

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            if (_cacheType.GetInterfaces().Contains(typeof(IDistributedCache)))
            {

            }

            //判断是否是异步方法
            bool isAsync = context.IsAsync();
            //if (context.ImplementationMethod.GetCustomAttribute(typeof(System.Runtime.CompilerServices.AsyncStateMachineAttribute)) != null)
            //{
            //    isAsync = true;
            //}
            //先判断方法是否有返回值，无就不进行缓存判断
            Type methodReturnType = context.GetReturnParameter().Type;
            if (methodReturnType == typeof(void) || methodReturnType == typeof(Task) || methodReturnType == typeof(ValueTask))
            {
                await next(context);
                return;
            }
            Type returnType = methodReturnType;
            if (isAsync)
            {
                //取得异步返回的类型
                returnType = returnType.GenericTypeArguments.FirstOrDefault();
            }
            //获取方法参数名
            //string param = CommonHelper.ObjectToJsonString(context.Parameters);
            ////获取方法名称，也就是缓存key值
            //string key = "Methods:" + context.ImplementationMethod.DeclaringType.FullName + "." + context.ImplementationMethod.Name;
            //var cache = context.ServiceProvider.GetService<ICacheHelper>();
        }
    }
}