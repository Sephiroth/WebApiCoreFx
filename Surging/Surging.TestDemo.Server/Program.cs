using Autofac;
using Microsoft.Extensions.Logging;
using Surging.Core.Caching.Configurations;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Configurations;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.Log4net;
using Surging.Core.ProxyGenerator;
using Surging.Core.ServiceHosting;
using Surging.Core.ServiceHosting.Internal.Implementation;
using Surging.DBModel.Models;
using Surging.EFCore.DBModel.SqlSugar;
using System;
using System.Text;

namespace Surging.TestDemo.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var host = new ServiceHostBuilder()
               .RegisterServices(builder =>
               {
                   builder.AddMicroService(option =>
                   {
                       option.AddServiceRuntime()
                       .AddRelateService()//添加支持服务代理远程调用
                       .AddConfigurationWatch()//添加同步更新配置文件的监听处理
                       //option.UseZooKeeperManager(new ConfigInfo("127.0.0.1:2181")); 
                       //.UseConsulManager(new ConfigInfo("127.0.0.1:8500"))//使用Consul管理
                       .AddServiceEngine(typeof(SurgingServiceEngine));
                       builder.Register(p => new CPlatformContainer(ServiceLocator.Current)); //初始化注入容器
                   });
               })
               .ConfigureLogging(logger =>
               {
                   logger.AddConfiguration(Surging.Core.CPlatform.AppConfig.GetSection("Logging"));
               })
               .UseServer(options =>
               {
                   //options.Ip = "127.0.0.1";
                   //options.Port = 2801;
                   //options.Token = "True";
                   //options.ExecutionTimeoutInMilliseconds = 30000;
                   //options.MaxConcurrentRequests = 200;
               })
               //.UseLog4net("log4net.config") //使用log4net记录日志
               //.UseLog4net(LogLevel.Error) //使用log4net记录日志
               //.UseNLog(LogLevel.Error, "NLog.config")
               .UseConsoleLifetime()
               .UseLog4net()
               .Configure(build => build.AddCacheFile("${cachepath}|cacheSettings.json", optional: false, reloadOnChange: true))
               .Configure(build => build.AddCPlatformFile("${surgingpath}|surgingSettings.json", optional: false, reloadOnChange: true))
               .UseStartup<Startup>()
               .Build();
            //在启动的时候吧连接字符串赋值
            SurgingtestContext.DbConnStr = Surging.Core.CPlatform.AppConfig.GetSection("ConnectionStrings").GetSection("MySqlStr").Value;
            SqlClient.ConnectionString = Surging.Core.CPlatform.AppConfig.GetSection("ConnectionStrings").GetSection("MySqlStr").Value;
            using (host.Run())
            {
                Console.WriteLine($"Surging.TestDemo服务端启动成功,{DateTime.Now}");
            }
        }
    }
}
