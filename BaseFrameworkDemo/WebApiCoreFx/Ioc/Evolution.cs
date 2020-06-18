using Autofac;
//using DBLayer.DAL;
//using IDBLayer.Interface;
using System.Reflection;

namespace WebApiCoreFx.Ioc
{
    public class Evolution : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注入Data层的Repository类
            //builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            // 通过反射批量注入Logic层的类
            //builder.RegisterAssemblyTypes(Assembly.Load("IDBLayer")).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces();
            // 注册程序集
            Assembly Service = Assembly.Load("LogicLayer");
            Assembly IService = Assembly.Load("ILogicLayer");
            builder.RegisterAssemblyTypes(IService, Service).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
        }
    }
}