using Autofac;
using Autofac.Extensions.DependencyInjection;
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

        /// <summary>
        /// 仅适用于.net core 3.0之前的版本
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assemblys"></param>
        /// <returns></returns>
        [Obsolete("仅适用于.net core 3.0之前的版本,3.0开始废弃此方法")]
        public static IServiceCollection IocByAutofac(this IServiceCollection services, string[] assemblys)
        {
            services.AddAutofac(builder =>
            {
                if (assemblys != null
                    && assemblys.Length == 2
                    && !assemblys[0].Equals(assemblys[1]))
                {
                    Assembly interfaceAmb = Assembly.Load(assemblys[0]);
                    Assembly implAmb = Assembly.Load(assemblys[1]);
                    builder.RegisterAssemblyTypes(interfaceAmb, implAmb);
                    builder.Build();
                }
            });
            return services;
        }

        /// <summary>
        /// 通过程序集名称
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblys">两个程序集的名称:0-接口程序集;1-实现程序集</param>
        /// <returns></returns>
        public static ContainerBuilder RegisterAssemblys(this ContainerBuilder builder, string[] assemblys)
        {
            if (assemblys != null
                && assemblys.Length == 2
                && !assemblys[0].Equals(assemblys[1]))
            {
                Assembly interfaceAmb = Assembly.Load(assemblys[0]);
                Assembly implAmb = Assembly.Load(assemblys[1]);
                builder.RegisterAssemblyTypes(interfaceAmb, implAmb);
            }
            return builder;
        }
    }
}