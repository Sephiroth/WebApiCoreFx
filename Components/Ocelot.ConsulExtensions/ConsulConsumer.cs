using Consul;
using Microsoft.Extensions.Options;
using Ocelot.ConsulExtensions.Interface;
using Ocelot.ConsulExtensions.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ocelot.ConsulExtensions
{
    public class ConsulConsumer : IServiceConsumer //定义成接口，以后换其它的注册中心方便替换
    {
        private readonly string consulAddress;

        public ConsulConsumer(IOptions<ConsulConfig> serviceOptions)
        {
            consulAddress = serviceOptions.Value.ConsulAddress;
        }

        public async Task<List<string>> GetServices(string serviceName)
        {
            ConsulClient consulClient = new ConsulClient(configuration =>
            {
                //服务注册地址：集群中任意一个地址
                configuration.Address = new Uri(consulAddress);
            });
            QueryResult<CatalogService[]> result = await consulClient.Catalog.Service(serviceName);
            List<string> list = new List<string>();
            if (result?.Response != null && result?.Response.Length > 0)
            {
                foreach (var item in result.Response)
                {
                    list.Add($"{item.ServiceAddress}:{item.ServicePort}");
                }
            }
            return list;
        }

    }
}