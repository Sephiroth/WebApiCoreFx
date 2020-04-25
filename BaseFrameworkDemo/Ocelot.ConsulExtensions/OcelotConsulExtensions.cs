using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            services.AddOcelot().AddConsul().AddConfigStoredInConsul().AddPolly();
            return services;
        }

        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, ConsulEntity serviceEntity)//IApplicationLifetime lifetime,
        {
            //consul地址
            Action<ConsulClientConfiguration> configClient = (consulConfig) =>
            {
                consulConfig.Address = new Uri($"http://{serviceEntity.ConsulIP}:{ serviceEntity.ConsulPort}");
                consulConfig.Datacenter = "dc1";
            };
            //建立连接
            var consulClient = new ConsulClient(configClient);
            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册
                Interval = TimeSpan.FromSeconds(10),//健康监测
                HTTP = string.Format(serviceEntity.HealthAPI),//心跳检测地址
                Timeout = TimeSpan.FromSeconds(5)
            };
            //注册
            var registrtion = new AgentServiceRegistration()
            {

                Checks = new[] { httpCheck },
                ID = "Service_" + Guid.NewGuid().ToString(),//服务编号不可重复
                Name = serviceEntity.ServiceName,//服务名称
                Address = serviceEntity.ServiceIP,//ip地址
                Port = serviceEntity.ServicePort//端口

            };
            //注册服务
            consulClient.Agent.ServiceRegister(registrtion);
            return app;
        }

    }
}