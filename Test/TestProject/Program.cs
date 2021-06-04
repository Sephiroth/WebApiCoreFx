using CSRedis;
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
        static void Main(string[] args)
        {

        }

        public static int GetMaxArea(byte[] arr)
        {
            if (arr.Length == 2)
            {
                return arr[0] * arr[1];
            }
            int leftIdx = 0;
            int rightIdx = arr.Length - 1;
            int maxVal = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                leftIdx = i;
                int cur = arr[leftIdx] * arr[rightIdx];
                if (maxVal < cur)
                {
                    maxVal = cur;
                }
                int pre1 = leftIdx + 1;
                int pre2 = rightIdx - 1;
                if (arr[leftIdx] < arr[rightIdx])
                    leftIdx += 1;
                else
                    rightIdx -= 1;

            }
        }
        public static int GetMin(int a, int b)
        {
            if (a <= b) { return a; }
            return b;
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
