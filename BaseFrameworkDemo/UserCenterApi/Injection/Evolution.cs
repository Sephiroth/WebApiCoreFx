using Autofac;
//using DBLayer.DAL;
//using IDBLayer.Interface;

namespace UserCenterApi.Injection
{
    public class Evolution : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注入Data层的Repository类,对应程序集已迁到.net core3.1
            //builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            // 通过反射批量注入Logic层的类
            //builder.RegisterAssemblyTypes(Assembly.Load("IDBLayer")).Where(t => t.Name.EndsWith("Repository")).AsImplementedInterfaces();
        }
    }
}