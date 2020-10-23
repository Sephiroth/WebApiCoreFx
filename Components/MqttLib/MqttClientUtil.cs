using log4net;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Publishing;
using MQTTnet.Client.Receiving;
using MQTTnet.Formatter;
using MQTTnet.Protocol;
using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MqttLib
{
    public class MqttClientUtil
    {
        static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        IMqttClient mqttClient;
        private MqttClientOptions options;

        private string username;
        private string password;
        private string ip;
        private int port;
        private MqttProtocolVersion proVersion;
        private string clientID;
        private bool cleanSession;
        private uint communicationTimeout;
        private uint keepAlivePeriod;
        private uint keepAliveSendInterval;
        private uint maxPacketSize;

        public static MqttClientUtil Instance;


        public Action<string, MqttApplicationMessage> HandleMsg;

        /// <summary>
        /// 创建工具类
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="proVersion">默认V500</param>
        /// <param name="maxPacketSize">默认800k</param>
        /// <param name="clientID">默认Null,自动生成</param>
        /// <param name="cleanSession">默认false</param>
        /// <param name="communicationTimeout">默认10s</param>
        /// <param name="keepAlivePeriod">默认20</param>
        /// <param name="keepAliveSendInterval">默认20</param>
        public MqttClientUtil(string ip,
            int port,
            string username,
            string password,
            MqttProtocolVersion proVersion = MqttProtocolVersion.V500,
            uint maxPacketSize = 819200,
            string clientID = null,
            bool cleanSession = false,
            uint communicationTimeout = 10,
            uint keepAlivePeriod = 20) //uint keepAliveSendInterval = 20
        {
            this.ip = ip;
            this.port = port;
            this.username = username;
            this.password = password;
            this.proVersion = proVersion;
            this.maxPacketSize = maxPacketSize;
            this.clientID = clientID ?? Guid.NewGuid().ToString("D");
            this.cleanSession = cleanSession;
            this.communicationTimeout = communicationTimeout;
            this.keepAlivePeriod = keepAlivePeriod;
            //this.keepAliveSendInterval = keepAliveSendInterval;
        }

        private bool InitClient()
        {
            mqttClient?.Dispose();
            options = new MqttClientOptions
            {
                ClientId = clientID,
                ProtocolVersion = proVersion,
                CleanSession = cleanSession,
                CommunicationTimeout = TimeSpan.FromSeconds(communicationTimeout),
                KeepAlivePeriod = TimeSpan.FromSeconds(keepAlivePeriod),
                //KeepAliveSendInterval = TimeSpan.FromSeconds(keepAliveSendInterval),
                MaximumPacketSize = maxPacketSize
            };

            options.ChannelOptions = new MqttClientTcpOptions { Server = ip, Port = port };
            options.Credentials = new MqttClientCredentials { Username = username, Password = Encoding.UTF8.GetBytes(password) };

            mqttClient = new MqttFactory().CreateMqttClient();
            mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
            {
                HandleMsg?.Invoke(e.ClientId, e.ApplicationMessage);
            });
            mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(e =>
            {
                Logger.Info("connect success!");
            });
            mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(e =>
            {
                Logger.Info($"clientID:{e.ClientWasConnected} disconnect!");
            });
            return true;
        }

        public async Task<bool> StartUpClientAsync()
        {
            _ = InitClient();
            MqttClientAuthenticateResult mqttRs = await mqttClient.ConnectAsync(options);
            return mqttRs.ResultCode == MqttClientConnectResultCode.Success;
        }

        public async Task StopClientAsync()
        {
            await mqttClient.DisconnectAsync();
        }

        public async Task<MqttClientPublishResult> Publish(string topic, byte[] data, MqttQualityOfServiceLevel level = MqttQualityOfServiceLevel.AtMostOnce)
        {
            bool rs = mqttClient.IsConnected;
            if (!rs) // 掉线重连
            {
                MqttClientAuthenticateResult mqttRs = await mqttClient.ConnectAsync(options);
                rs = mqttRs.ResultCode == MqttClientConnectResultCode.Success;
            }
            if (rs)
            {
                MqttApplicationMessage msg = new MqttApplicationMessage
                {
                    Topic = topic,
                    Payload = data,
                    QualityOfServiceLevel = level,
                    Retain = false
                };
                return await mqttClient.PublishAsync(msg);
            }
            return new MqttClientPublishResult { ReasonCode = MqttClientPublishReasonCode.UnspecifiedError };
        }

    }
}