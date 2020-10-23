using log4net;
using MQTTnet;
using MQTTnet.Client.Publishing;
using MQTTnet.Diagnostics;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace MqttLib
{
    public class MqttServerUtil
    {
        static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static MqttServerUtil Instance;

        private MqttServer mqttServer;
        private MqttServerOptionsBuilder serverOptionsBuilder;
        private IMqttServerOptions serverOptions;

        private string username;
        private string password;
        private string ip;
        private int port;
        private SslProtocols sslProtocols;
        private uint communicationTimeout;

        public SortedSet<string> ClientIds { get; }
        public Action<string, MqttApplicationMessage> AppMsgReceivedHandler { get; set; }

        public MqttServerUtil(string ip, int port, string username, string password,
            uint communicationTimeout = 20, SslProtocols sslProtocols = SslProtocols.None)
        {
            ClientIds = new SortedSet<string>();
            this.ip = ip;
            this.port = port;
            this.username = username;
            this.password = password;
            this.communicationTimeout = communicationTimeout;
            this.sslProtocols = sslProtocols;
        }

        private void InitServer()
        {
            IMqttNetLogger logger = new MqttNetLogger();

            serverOptionsBuilder = new MqttServerOptionsBuilder()
                .WithDefaultEndpointBoundIPAddress(IPAddress.Parse(ip))
                .WithDefaultEndpointPort(port)
                .WithDefaultCommunicationTimeout(TimeSpan.FromSeconds(communicationTimeout))
                .WithEncryptionSslProtocol(sslProtocols);
            // connect verfication
            serverOptionsBuilder.WithConnectionValidator(valid =>
            {
                if (valid.ClientId.Length < 10)
                {
                    valid.ReasonCode = MqttConnectReasonCode.ClientIdentifierNotValid;
                    return;
                }
                if (!valid.Username.Equals(this.username))
                {
                    valid.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                    return;
                }
                if (!valid.Password.Equals(password))
                {
                    valid.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
                    return;
                }
                valid.ReasonCode = MqttConnectReasonCode.Success;
            });
            serverOptions = serverOptionsBuilder.Build();

            mqttServer = (new MqttFactory()).CreateMqttServer(logger) as MqttServer;
            mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(e =>
            {
                ClientIds.Add(e.ClientId);
                Logger.Info($"clientID:{e.ClientId} connect success!");
            });
            mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(e =>
            {
                ClientIds.RemoveWhere(s => s.Equals(e.ClientId));
                Logger.Info($"clientID:{e.ClientId} disconnect!");
            });
            mqttServer.UseApplicationMessageReceivedHandler(e =>
            {
                AppMsgReceivedHandler?.Invoke(e.ClientId, e.ApplicationMessage);
            });
        }

        public async Task StartUpServer()
        {
            if (mqttServer == null)
                InitServer();
            await mqttServer.StartAsync(serverOptions);
        }

        public async Task StopAsync()
        {
            await mqttServer.StopAsync();
        }

        public async Task<MqttClientPublishResult> Publish(string topic, byte[] data, MqttQualityOfServiceLevel level = MqttQualityOfServiceLevel.AtMostOnce)
        {
            MqttApplicationMessage msg = new MqttApplicationMessage
            {
                Topic = topic,
                Payload = data,
                QualityOfServiceLevel = level,
                Retain = false
            };
            return await mqttServer.PublishAsync(msg);
        }

    }
}