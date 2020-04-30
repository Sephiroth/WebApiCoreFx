using Castle.DynamicProxy.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;

namespace WebApiCoreFx.Ioc
{
    public static class IocExtension
    {
        /// <summary>
        /// 基于Microsoft.Extensions.DependencyInjection的AddTransient拓展方法注册程序集
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblys"></param>
        /// <returns></returns>
        public static IServiceCollection IocAssembly(this IServiceCollection services, string[] assemblys)
        {
            if (assemblys != null
                && assemblys.Length == 2
                && !assemblys[0].Equals(assemblys[1]))
            {
                Assembly interfaceAmb = Assembly.Load(assemblys[0]);
                Assembly implAmb = Assembly.Load(assemblys[1]);
                var typesInterface = interfaceAmb.GetTypes();
                var typesImpl = implAmb.GetTypes();

                foreach (Type item in typesInterface)
                {
                    foreach (Type impl in typesImpl)
                    {
                        bool? hadObj = impl.GetAllInterfaces()?.Contains(item);
                        if (hadObj.HasValue && hadObj.Value)
                        {
                            services.AddTransient(item, impl);
                        }
                    }
                }
            }
            return services;
        }
    }
}