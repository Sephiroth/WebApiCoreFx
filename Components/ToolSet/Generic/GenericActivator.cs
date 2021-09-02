using System;
using System.Linq.Expressions;

namespace ToolSet.Generic
{
    public class GenericActivator
    {
        public static T CreateInstance<T>() where T : new()
        {
            return GenericActivatorImpl<T>.NewFunction();
        }

        private class GenericActivatorImpl<T> where T : new()
        {
            // 利用表达式树就会“提示”编译器不会调用 Activator.CreateInstance 
            private static readonly Expression<Func<T>> NewExpression = () => new T();
            internal static readonly Func<T> NewFunction = NewExpression.Compile();
        }
    }

    /// <summary>
    /// 高效创建泛型对象(eg: GenericActivator<T>.Create())
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class GenericActivator<T> where T : new()
    {
        public static readonly Func<T> Create = DynamicModuleLambdaCompiler.GenerateFactory<T>();
    }

}