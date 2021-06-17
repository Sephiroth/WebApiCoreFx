using MassTransit;
using MassTransit.ActiveMqTransport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBus
{
    public static class MessageBusExtensions
    {
        /// <summary>
        /// 添加使用ActiveMQ作为消息总线代理
        /// </summary>
        /// <param name="services"></param>
        /// <param name="info">AvtiveMQ连接信息</param>
        /// <param name="receiveKvs">maps<队列,消费者></param>
        /// <returns></returns>
        public static IServiceCollection AddMassTransitByActiveMQ(this IServiceCollection services, ConnectionInfo info, IDictionary<string, Action<IReceiveEndpointConfigurator>> receiveKvs = null)
        {
            services.AddMassTransit(x =>
            {
                x.UsingActiveMq((a, b) =>
                {
                    b.Host(info.IP, info.Port, h =>
                    {
                        h.Username(info.User);
                        h.Password(info.Pwd);
                    });
                    if (receiveKvs != null)
                    {
                        foreach (KeyValuePair<string, Action<IReceiveEndpointConfigurator>> kv in receiveKvs)
                        {
                            b.ReceiveEndpoint(kv.Key, kv.Value);
                        }
                    }
                });
                x.SetKebabCaseEndpointNameFormatter();
            });
            return services.AddMassTransitHostedService();
        }

        /// <summary>
        /// 添加使用RabbitMQ作为消息总线代理
        /// </summary>
        /// <param name="services"></param>
        /// <param name="info"></param>
        /// <param name="receiveKvs">maps<队列,消费者></param>
        /// <returns></returns>
        public static IServiceCollection AddMassTransitByRabbitMQ(this IServiceCollection services, ConnectionInfo info, IDictionary<string, Action<IReceiveEndpointConfigurator>> receiveKvs = null)
        {
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((a, b) =>
                {
                    b.Host(info.IP, info.Port, info.VirtualHost, c =>
                    {
                        c.Username(info.User);
                        c.Password(info.Pwd);
                    });
                    if (receiveKvs != null)
                    {
                        foreach (KeyValuePair<string, Action<IReceiveEndpointConfigurator>> kv in receiveKvs)
                        {
                            b.ReceiveEndpoint(kv.Key, kv.Value);
                        }
                    }
                });
                x.SetKebabCaseEndpointNameFormatter();
            });
            return services.AddMassTransitHostedService();
        }

        // RabbitMQ
        //configure.UsingRabbitMq((context, configurator) =>
        //{
        //    configurator.Host("192.168.52.128", 5672, "/", hostConf =>
        //     {
        //         hostConf.Username("guest");
        //         hostConf.Password("guest");
        //         hostConf.Heartbeat(5);
        //         hostConf.RequestedChannelMax(5);
        //         hostConf.RequestedConnectionTimeout(10000);
        //     });
        //    configurator.ReceiveEndpoint("reveiveQueue", e =>
        //    {
        //        e.Consumer<model.BusMsgConsumer>();
        //    });
        //});

    }
}