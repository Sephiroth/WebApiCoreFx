namespace MessageBus
{
    public class BusConnectionInfo : ConnectionInfo
    {
        public string IP { get; set; }
        public ushort Port { get; set; }
        public string User { get; set; }
        public string Pwd { get; set; }
        public string VirtualHost { get; set; }
    }
}