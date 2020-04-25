namespace Ocelot.ConsulExtensions.Model
{
    internal class ServiceDisvoveryOptions
    {
        public string ServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }
}