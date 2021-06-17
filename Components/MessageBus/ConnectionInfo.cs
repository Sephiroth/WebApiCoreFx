namespace MessageBus
{
    public class ConnectionInfo
    {
        public string IP { get; set; }
        public ushort Port { get; set; }
        public string User { get; set; }
        public string Pwd { get; set; }
        /// <summary>
        /// RabitMQ虚拟主机
        /// </summary>
        public string VirtualHost { get; set; } = "/";
    }
}