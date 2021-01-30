using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.ConsulExtensions.Interface;
using Ocelot.ConsulExtensions.Model;
using System;
using System.Collections.Generic;
using System.Text;

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
                throw new Exception("请正确配置appsettings.json");
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