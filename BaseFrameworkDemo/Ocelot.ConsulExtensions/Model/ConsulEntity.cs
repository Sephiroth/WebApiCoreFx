namespace Ocelot.ConsulExtensions.Model
{
    public class ConsulEntity
    {
        /// <summary>
        /// 本机服务IP
        /// </summary>
        public string HealthAPI { get; set; }
        public string ServiceName { get; set; }
        public string ServiceIP { get; set; }
        public int ServicePort { get; set; }
        public string ConsulIP { get; set; }
        public int ConsulPort { get; set; }
    }
}