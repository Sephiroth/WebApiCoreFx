using App.Metrics;
using DnsClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ocelot.ConsulExtensions;
using Ocelot.DependencyInjection;
using Ocelot.GatewayProj.Filter;
using Ocelot.GatewayProj.Model;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using System;

namespace Ocelot.GatewayProj
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }
        private string authenticationProviderKey;
        private bool isOpenMetrics;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration) // IWebHostEnvironment env
        {
            //ConfigurationBuilder config = new ConfigurationBuilder();
            //config.SetBasePath(env.ContentRootPath).AddJsonFile("Ocelot.json").AddEnvironmentVariables();
            //Configuration = config.Build();

            authenticationProviderKey = Configuration["ReRoutes:0:AuthenticationOptions:AuthenticationProviderKey"];
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 跨域
            services.AddCors(options =>
            {
                string[] corsOrigins = Configuration.GetSection("CorsOrigins").Get<string[]>();
                options.AddPolicy("AllowAll", p =>
                {
                    // 允许所有的域名请求 // 允许所有的请求方式GET/POST/PUT/DELETE // 允许所有的头部参数 // 允许携带Cookie
                    // .AllowAnyOrigin().AllowCredentials()
                    p.WithOrigins(corsOrigins).AllowAnyMethod().AllowAnyHeader();
                });
            });
            #endregion

            services.AddControllers(options =>
            {
                options.Filters.Add<HttpGlobalExceptionFilter>();
                options.EnableEndpointRouting = true;
            });

            //从配置文件中获取ServiceDiscovery
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));

            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            });

            services.AddConsulConsumer();

            services.AddSingleton<IDnsQuery>(p =>
            {
                //从配置文件中获取consul相关配置信息
                ServiceDisvoveryOptions serviceConfig = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;
                return new LookupClient(serviceConfig.Consul.DnsEndpoint.ToIPEndPoint());
            });

            #region influxDB的配置 用于存储网关访问数据
            isOpenMetrics = Configuration.GetValue<bool>("IsOpen");
            if (isOpenMetrics)
            {
                string database = Configuration.GetValue<string>("DatabaseName");
                string connStr = Configuration.GetValue<string>("ConnectionString");
                string app = Configuration.GetValue<string>("App");
                string env = Configuration.GetValue<string>("Env");
                string username = Configuration.GetValue<string>("UserName");
                string password = Configuration.GetValue<string>("Password");
                Uri uri = new Uri(connStr);
                IMetricsRoot metrics = AppMetrics.CreateDefaultBuilder().Configuration.Configure(options =>
                {
                    options.AddAppTag(app);
                    options.AddEnvTag(env);
                })
                    .Report.ToInfluxDb(options =>
                    {
                        options.InfluxDb.BaseUri = uri;
                        options.InfluxDb.Database = database;
                        options.InfluxDb.UserName = username;
                        options.InfluxDb.Password = password;
                        options.HttpPolicy.BackoffPeriod = TimeSpan.FromSeconds(30);
                        options.HttpPolicy.FailuresBeforeBackoff = 5;
                        options.HttpPolicy.Timeout = TimeSpan.FromSeconds(10);
                        options.FlushInterval = TimeSpan.FromSeconds(5);
                    })
                    .Build();

                services.AddMetrics(metrics);
                //services.AddMetricsReportScheduler();
                services.AddMetricsTrackingMiddleware();
                services.AddMetricsEndpoints();
            }
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (isOpenMetrics)
            {
                app.UseMetricsAllEndpoints();
                app.UseMetricsAllMiddleware();
            }
            app.UseCors("AllowAll");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();//.RequireCors("AllowAll");
                endpoints.MapControllerRoute("default", "api/{controller=Home}/{action=Index}");
            });

            app.UseOcelot().Wait();
        }
    }
}