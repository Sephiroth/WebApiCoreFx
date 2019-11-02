using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace UserCenterApi.Extensions
{
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// 把服务注册到Consul
        /// </summary>
        /// <param name="app"></param>
        /// <param name="config"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IConfiguration config, IApplicationLifetime lifetime)
        {
            string consulIP = config.GetValue<string>("Consul:IP");
            string consulPort = config.GetValue<string>("Consul:Port");
            string serviceName = config.GetValue<string>("Consul:ServiceName");
            string consulHost = config.GetValue<string>("Consul:Host");
            //请求注册的 Consul 地址
            var consulClient = new ConsulClient(x => x.Address = new Uri(consulHost));
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔
                HTTP = $"{consulHost}/api/health",//健康检查地址
                Timeout = TimeSpan.FromSeconds(5)
            };

            // Register service with consul
            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = serviceName,
                Address = consulIP,
                Port = Convert.ToInt32(consulPort),
                Tags = new[] { $"urlprefix-/{serviceName}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
            });
            return app;
        }
    }
}