using CSRedis;
using MassTransit;
using MQHelper.RabbitMQ;
using RabbitMQ.Client;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Authentication;
using System.Text;

namespace TestProject
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Dictionary<string, Action<MassTransit.IReceiveEndpointConfigurator>> dic = new Dictionary<string, Action<MassTransit.IReceiveEndpointConfigurator>>
            {
                { "testQueue", (conf) => { conf.Consumer<MessageBus.MsgConsumer>();} }
            };

            MessageBus.MessageBusClient client = new MessageBus.MessageBusClient(
                MessageBus.MessageBusType.ActiveMQ,
                new MessageBus.ConnectionInfo
                {
                    IP = "10.1.72.200",
                    Port = 61616,
                    User = "admin",
                    Pwd = "admin"
                },
                dic);
            await client.StartAsync();
            bool rs = await client.SendAsync("testQueue", new MessageBus.Msg { Text = "汉字随便测试" });

            Console.ReadLine();
        }

        public static RabbitMqUtil NewMqClient(int number)
        {
            RabbitMqUtil rabbit = new RabbitMqUtil();
            var conn = rabbit.CreateConnection(new ConnectionFactory
            {
                HostName = "192.168.1.11",
                Port = 5676,
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                //Protocol = Protocols.DefaultProtocol,
                AmqpUriSslProtocols = SslProtocols.Tls,
                AutomaticRecoveryEnabled = true, //自动重连
                RequestedFrameMax = UInt32.MaxValue,
                RequestedHeartbeat = TimeSpan.FromSeconds(UInt16.MaxValue) //心跳超时时间
            });
            string queue = $"queue{number}";
            IModel model = rabbit.CreateModel(conn);
            model.DeclareQueue(queue, "ChatExchange", "", false, false, true);
            model.AddConsumer(queue, RcvMsg);
            System.Threading.Tasks.Task.Factory.StartNew(async () =>
           {
               for (int i = 0; i < 500; i++)
               {
                   byte[] bs = Encoding.UTF8.GetBytes($"我是{queue},第{i}次");
                   await System.Threading.Tasks.Task.Delay(3000);
                   model.BasicPublish("ChatExchange", "", false, null, bs);
               }
           });
            return rabbit;
        }

        public static void RcvMsg(object sender, RabbitMQ.Client.Events.BasicDeliverEventArgs args)
        {
            Console.WriteLine($"{args.ConsumerTag}:{Encoding.UTF8.GetString(args.Body.ToArray())}");
        }

    }
}
