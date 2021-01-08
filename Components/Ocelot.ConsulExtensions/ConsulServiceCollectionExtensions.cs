using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.ConsulExtensions.Interface;
using Ocelot.ConsulExtensions.Model;
using System;

namespace Ocelot.ConsulExtensions
{
    public static class ConsulServiceCollectionExtensions
    {
        public static void AddConsulRegister(this IServiceCollection service)
        {
            //读取服务配置文件
            try
            {
                var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build(); //nuget: Microsoft.Extensions.Configuration.Json
                service.Configure<ConsulConfig>(config.GetSection("Consul")); // nuget： Microsoft.Extensions.Options.ConfigurationExtensions
            }
            catch
            {
                throw new Exception("请正确配置appsettings.json,根节点添加\"consul\":{\r\n"
  + "\"ConsulAddress\": \"http://192.168.233.128:8500\", //Consul服务注册地址，如果是消费者，则只需要配置这个字段，其它的无需配置 \r\n "
  + "\"ServiceName\": \"UserCenterApi\", //当前服务名称，可以多个实例共享 \r\n"
  + "\"ServiceUriHost\": \" http://10.1.72.24 \", //当前服务地址 \r\n"
  + "\"ServiceUriPort\": \"5000\", //当前服务地址 \r\n"
  + "\"HealthCheck\": \"/api/health/check \" //健康检查的地址，当前服务公布出来的一个api接口 \r\n"
  + "}"
            );
            }
        }

        public static void AddConsulConsumer(this IServiceCollection service)
        {
            //读取服务配置文件
            service.AddConsulRegister(); // nuget： Microsoft.Extensions.Options.ConfigurationExtensions
            service.AddSingleton<IServiceConsumer, ConsulConsumer>();
        }

    }
}