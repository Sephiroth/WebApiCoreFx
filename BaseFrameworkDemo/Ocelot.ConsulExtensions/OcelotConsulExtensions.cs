using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.ConsulExtensions.Model;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using System;

namespace Ocelot.ConsulExtensions
{
    public static class OcelotConsulExtensions
    {
        public static IServiceCollection AddOcelotConsul(this IServiceCollection services)
        {
            services.AddOcelot().AddConsul().AddConfigStoredInConsul();//.AddPolly();
            return services;
        }

        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IConfiguration configuration)//IApplicationLifetime lifetime,
        {
            ConsulEntity consulEntity = new ConsulEntity
            {
                HealthAPI = configuration["HealthAPI"],
                ServiceIP = configuration["Service:IP"],
                ServicePort = Convert.ToInt32(configuration["Service:Port"]),
                ServiceName = configuration["Service:Name"],
                ConsulIP = configuration["Consul:IP"],
                ConsulPort = Convert.ToInt32(configuration["Consul:Port"])
            };
            //consul地址
            Action<ConsulClientConfiguration> configClient = (consulConfig) =>
            {
                consulConfig.Address = new Uri($"http://{consulEntity.ConsulIP}:{ consulEntity.ConsulPort}");
                consulConfig.Datacenter = "dc1";
            };
            //建立连接
            ConsulClient consulClient = new ConsulClient(configClient);
            AgentServiceCheck httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康监测
                HTTP = string.Format(consulEntity.HealthAPI),//心跳检测地址
                Timeout = TimeSpan.FromSeconds(5)
            };
            //注册
            AgentServiceRegistration registrtion = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = "Service_" + Guid.NewGuid().ToString(),//服务编号不可重复
                Name = consulEntity.ServiceName,//服务名称
                Address = consulEntity.ServiceIP,//ip地址
                Port = consulEntity.ServicePort//端口
            };
            //注册服务
            consulClient.Agent.ServiceRegister(registrtion).Wait();
            return app;
        }

    }
}