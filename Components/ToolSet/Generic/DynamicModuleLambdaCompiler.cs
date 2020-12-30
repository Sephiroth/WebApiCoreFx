using System;
using System.Linq.Expressions;
using System.Reflection.Emit;

namespace ToolSet.Generic
{
    /// <summary>
    /// 动态lambda编译器
    /// </summary>
    class DynamicModuleLambdaCompiler
    {
        public static Func<T> GenerateFactory<T>() where T : new()
        {
            Expression<Func<T>> expr = () => new T();
            NewExpression newExpr = (NewExpression)expr.Body;

            DynamicMethod method = new DynamicMethod(
                name: "lambda",
                returnType: typeof(T),
                parameterTypes: new Type[0],
                m: typeof(DynamicModuleLambdaCompiler).Module,
                skipVisibility: true
                );

            ILGenerator iLGen = method.GetILGenerator();
            if (newExpr.Constructor != null)
            {
                iLGen.Emit(OpCodes.Newobj, newExpr.Constructor);
            }
            else
            {
                LocalBuilder temp = iLGen.DeclareLocal(typeof(T));
                iLGen.Emit(OpCodes.Ldloca, temp);
                iLGen.Emit(OpCodes.Initobj, newExpr.Type);
                iLGen.Emit(OpCodes.Ldloc, temp);
            }
            iLGen.Emit(OpCodes.Ret);
            return (Func<T>)method.CreateDelegate(typeof(Func<T>));
        }

    }
}