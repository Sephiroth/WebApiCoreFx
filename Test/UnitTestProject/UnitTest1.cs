using Microsoft.VisualStudio.TestTools.UnitTesting;
using MQHelper.RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Text;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        private RabbitMqUtil sendMq;
        private RabbitMqUtil rcvMq;

        public UnitTest1()
        {
            sendMq = new RabbitMqUtil("guest", "guest", "TestExchange", "TestQueue1", "routingkey1", false, ExchangeType.Topic, false, "192.168.194.128", 5673);
            sendMq.InitMqCreateExchangeQueue(ReturnHandler, ReceiveHandler);
            rcvMq = new RabbitMqUtil("guest", "guest", "TestExchange", "TestQueue", "rkey1", false, ExchangeType.Topic, false, "192.168.194.128", 5675);
            
        }

        private void ReceiveHandler(object sender,RabbitMQ.Client.Events.BasicDeliverEventArgs args)
        {
            string rs = Encoding.UTF8.GetString(args.Body.ToArray());
            Console.WriteLine(rs);
            rcvMq.Ack(args.DeliveryTag);
        }

        private void ReturnHandler(object sender, RabbitMQ.Client.Events.BasicReturnEventArgs args)
        {
            string rs = Encoding.UTF8.GetString(args.Body.ToArray());
            Console.WriteLine(rs);
        }

        [TestMethod]
        public void TestMethod1()
        {
            for (int i = 0; i < 100; i++)
            {
                string msg = $"{i}:ÏûÏ¢{i}";
                sendMq.Send(Encoding.UTF8.GetBytes(msg), "routingkey1");
            }
        }
    }
}
