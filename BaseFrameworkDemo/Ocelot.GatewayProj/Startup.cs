using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using CacheManager.Core.Utility;
using Ocelot.Cache.CacheManager;
using Ocelot.Cache;
using Ocelot.GatewayProj.Model;
using DnsClient;
using Microsoft.Extensions.Options;

namespace Ocelot.GatewayProj
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        private string authenticationProviderKey;

        public Startup(IHostingEnvironment env)
        {
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            config.SetBasePath(env.ContentRootPath)
                .AddJsonFile("Ocelot.json")
                .AddEnvironmentVariables();
            Configuration = config.Build();

            authenticationProviderKey = Configuration["ReRoutes:0:AuthenticationOptions:AuthenticationProviderKey"];
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //从配置文件中获取ServiceDiscovery
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));


            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            });
            services.AddOcelot()
                .AddCacheManager(s => { s.WithDictionaryHandle(); })
                .AddConsul();//.AddTransientDefinedAggregator<FakeDefinedAggregator>()
            services.AddAuthentication().AddJwtBearer(authenticationProviderKey, s =>
            {
            });
            // 使用自定义缓存
            //services.AddSingleton<IOcelotCache<CachedResponse>, MyCache>();

            services.AddSingleton<IDnsQuery>(p =>
            {
                //从配置文件中获取consul相关配置信息
                ServiceDisvoveryOptions serviceConfig = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;
                return new LookupClient(serviceConfig.Consul.DnsEndpoint.ToIPEndPoint());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseOcelot().Wait();

            //app.UseHttpsRedirection();
            //app.UseMvc();
        }
    }
}