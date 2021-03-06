﻿using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Ocelot.ConsulExtensions.Model;
using System;
using System.Linq;

namespace Ocelot.ConsulExtensions
{
    /// <summary>
    /// 服务注册
    /// </summary>
    public static class ConsulRegister
    {
        public static IApplicationBuilder UseConsul(this IApplicationBuilder app)
        {
            //获取服务配置项
            ConsulConfig serviceOptions = app.ApplicationServices.GetRequiredService<IOptions<ConsulConfig>>().Value;
            CheckConfig(serviceOptions);
            string checkUrl = $"{serviceOptions.ServiceUriHost}:{serviceOptions.ServiceUriPort}{serviceOptions.HealthCheck}";
            //Tuple<string, string, int> hostinfo = GetHostInfo(serviceOptions, app);
            // 服务ID，唯一的
            string serviceId = $"{serviceOptions.ServiceName}_{Guid.NewGuid()}";
            //节点服务注册对象
            AgentServiceRegistration registration = new AgentServiceRegistration()
            {
                ID = serviceId,
                Name = serviceOptions.ServiceName,  //对服务分组
                Address = serviceOptions.ServiceUriHost, //服务地址
                Port = serviceOptions.ServiceUriPort,
                Tags = new string[] { }, //标签信息，服务发现的时候可以获取到的，负载均衡策略扩展的
                Check = new AgentServiceCheck()
                {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(7),  //在7秒未连接上服务之后注销关键服务
                    Interval = TimeSpan.FromSeconds(2), //每个2秒发送一次心跳检测
                    Timeout = TimeSpan.FromSeconds(3),  //连接超时时间
                    HTTP = checkUrl                     //心跳检测访问的接口地址，需要自己在项目中写好这个接口
                }
            };

            ConsulClient consulClient = new ConsulClient(configuration =>
            {
                //服务注册地址：集群中任意一个地址
                configuration.Address = new Uri(serviceOptions.ConsulAddress);
            });
            //注册到consul
            consulClient.Agent.ServiceRegister(registration).Wait();
            //获取主机生命周期管理接口
            IHostApplicationLifetime lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            //程序停止的时候取消注册
            lifetime.ApplicationStopping.Register(() =>
            {
                consulClient.Agent.ServiceDeregister(serviceId).Wait();
            });

            return app;
        }

        /// <summary>
        /// 检查配置文件
        /// </summary>
        /// <param name="serviceOptions"></param>
        private static void CheckConfig(ConsulConfig serviceOptions)
        {
            if (serviceOptions == null)
                throw new Exception("请正确配置appsettings.json,其中包含ConsulAddress、ServiceName、HealthCheck");

            if (string.IsNullOrEmpty(serviceOptions.ConsulAddress))
                throw new Exception("请正确配置ConsulAddress");

            if (string.IsNullOrEmpty(serviceOptions.ServiceName))
                throw new Exception("请正确配置ServiceName");

            if (string.IsNullOrEmpty(serviceOptions.HealthCheck))
                throw new Exception("请正确配置HealthCheck");

        }

    }
}