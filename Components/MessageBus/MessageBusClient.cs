using MassTransit;
using MassTransit.ActiveMqTransport;
using MassTransit.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


namespace MessageBus
{
    public class MessageBusClient
    {
        public ConnectionInfo ConnectionInfo { get; protected set; }
        public MessageBusType BusType { get; protected set; }

        /// <summary>
        /// <监听队列,对应处理方法>
        /// </summary>
        private Dictionary<string, Action<IReceiveEndpointConfigurator>> _receiveDic;
        private Action<IReceiveEndpointConfigurator> _configureEndpoint = null;
        private IBusControl _busControl;
        private BusHandle _busHandler;

        private ConcurrentDictionary<string, ISendEndpoint> _sendEndpoints;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="busType">消息总线的实现方式</param>
        /// <param name="connectionInfo"></param>
        /// <param name="receiveDic"></param>
        public MessageBusClient(
            MessageBusType busType,
            ConnectionInfo connectionInfo,
            Dictionary<string, Action<IReceiveEndpointConfigurator>> receiveDic = null)
        {
            BusType = busType;
            ConnectionInfo = connectionInfo;
            _receiveDic = receiveDic ?? new Dictionary<string, Action<IReceiveEndpointConfigurator>>(0);
            _sendEndpoints = new ConcurrentDictionary<string, ISendEndpoint>();
        }

        public async ValueTask StartAsync()
        {
            _busControl ??= CreateBusControl();
            _busHandler ??= await _busControl.StartAsync();
        }

        public async ValueTask StopAsync()
        {
            await _busHandler.StopAsync();
        }

        public IBusControl CreateBusControl()
        {
            return BusType switch
            {
                MessageBusType.Memory => Bus.Factory.CreateUsingInMemory(cfg =>
                    {
                        foreach (KeyValuePair<string, Action<IReceiveEndpointConfigurator>> kv in _receiveDic)
                        {
                            cfg.ReceiveEndpoint(kv.Key, kv.Value);
                        }
                    }),
                MessageBusType.RabbitMQ => Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        cfg.Host(ConnectionInfo.IP, ConnectionInfo.Port, ConnectionInfo.VirtualHost, cnf =>
                        {
                            cnf.Username(ConnectionInfo.User);
                            cnf.Password(ConnectionInfo.Pwd);
                        });
                        foreach (KeyValuePair<string, Action<IReceiveEndpointConfigurator>> kv in _receiveDic)
                        {
                            cfg.ReceiveEndpoint(kv.Key, kv.Value);
                        }
                    }),
                MessageBusType.ActiveMQ => Bus.Factory.CreateUsingActiveMq(cfg =>
                    {
                        cfg.Host(ConnectionInfo.IP, ConnectionInfo.Port, cnf =>
                        {
                            cnf.Username(ConnectionInfo.User);
                            cnf.Password(ConnectionInfo.Pwd);
                        });
                        foreach (KeyValuePair<string, Action<IReceiveEndpointConfigurator>> kv in _receiveDic)
                        {
                            cfg.ReceiveEndpoint(kv.Key, kv.Value);
                        }
                    }),
                _ => null,
            };
        }

        public async ValueTask<bool> SendAsync(string sencQueue, object msg)
        {
            bool exist = _sendEndpoints.TryGetValue(sencQueue, out ISendEndpoint sendEndpoint);
            if (!exist)
            {
                sendEndpoint = await _busControl.GetSendEndpoint(new Uri($"activemq://{ConnectionInfo.IP}:{ConnectionInfo.Port}/{sencQueue}"));
                exist = _sendEndpoints.TryAdd(sencQueue, sendEndpoint);
                if (!exist) { return false; }
            }
            await sendEndpoint.Send(msg);
            return true;
        }

        static void Main()
        {

            string myPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "aa.mp3");

            Console.WriteLine("目录名称：{0}", Path.GetDirectoryName(myPath));

            Console.WriteLine("路径扩展名：{0}", Path.GetExtension(myPath));

            Console.WriteLine("文件名：{0}", Path.GetFileName(myPath));

            Console.WriteLine("不带扩展名的名称：{0}", Path.GetFileNameWithoutExtension(myPath));

            Console.WriteLine("绝对全路径：{0}", Path.GetFullPath(myPath));

            Console.WriteLine("根目录：{0}", Path.GetPathRoot(myPath));

            Console.WriteLine("不带根目录的路径：{0}", Path.GetFullPath(myPath).Remove(0, 3));

            Console.ReadKey();
            Dictionary<string, Action<IReceiveEndpointConfigurator>> dic = new Dictionary<string, Action<IReceiveEndpointConfigurator>>();
            dic.Add("testQueue", x =>
            {
                x.Consumer<MsgConsumer>();
            });
            MessageBusClient busClient = new MessageBusClient(MessageBusType.Memory, null, dic);
        }

        class Msg { public string Text { get; set; } }
        class MsgConsumer : IConsumer<Msg>
        {
            public Task Consume(ConsumeContext<Msg> context)
            {
                return Task.Factory.StartNew(() => { Console.WriteLine(context.Message.Text); });
            }
        }
    }
}