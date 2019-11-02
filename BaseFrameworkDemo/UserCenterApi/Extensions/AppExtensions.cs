using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace UserCenterApi.Extensions
{
    public static class AppExtensions
    {
        private static string ServiceName;

        public static IServiceCollection AddConsulConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig =>
            {
                string address = configuration.GetValue<string>("Consul:Host");
                ServiceName = configuration.GetValue<string>("Consul:ServiceName");
                consulConfig.Address = new Uri(address);
            }));
            return services;
        }

        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            var consulClient = app.ApplicationServices.GetRequiredService<IConsulClient>();
            var logger = app.ApplicationServices.GetRequiredService<ILoggerFactory>().CreateLogger("AppExtensions");
            var lifetime = app.ApplicationServices.GetRequiredService<IApplicationLifetime>();

            if (!(app.Properties["server.Features"] is FeatureCollection features))
                return app;

            var addresses = features.Get<IServerAddressesFeature>();
            var address = addresses.Addresses.First();

            Console.WriteLine($"address={address}");

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"{address}/health",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };

            var uri = new Uri(address);
            var registration = new AgentServiceRegistration()
            {
                ID = $"{ServiceName}-{uri.Port}",
                Name = ServiceName,
                Address = $"{uri.Host}",
                Port = uri.Port,
                Checks = new[] { httpCheck },
                //添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
                Tags = new[] { $"urlprefix-/{ServiceName}" }
            };

            logger.LogInformation("Registering with Consul");
            //consulClient.Agent.ServiceDeregister(registration.ID).ConfigureAwait(true);
            //服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            consulClient.Agent.ServiceRegister(registration).Wait();
            lifetime.ApplicationStopping.Register(() =>
            {
                logger.LogInformation("Unregistering from Consul");
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
            });

            return app;
        }
    }
}