using System.Net;

namespace Ocelot.ConsulExtensions.Model
{
    internal class ConsulOptions
    {
        public string HttpEndpoint { get; set; }

        public DnsEndPoint DnsEndpoint { get; set; }
    }
}