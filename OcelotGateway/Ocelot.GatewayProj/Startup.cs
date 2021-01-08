using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.GatewayProj.Filter;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;

namespace Ocelot.GatewayProj
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration) // IWebHostEnvironment env
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 跨域
            services.AddCors(options =>
            {
                //string[] corsOrigins = Configuration.GetSection("CorsOrigins").Get<string[]>();
                options.AddPolicy("AllowAll", p =>
                {
                    // 允许所有的域名请求 // 允许所有的请求方式 GET/POST/PUT/DELETE // 允许所有的头部参数 // 允许携带Cookie
                    // .AllowAnyOrigin().AllowCredentials()
                    //p.WithOrigins(corsOrigins).AllowAnyMethod().AllowAnyHeader();
                    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyMethod();
                });
            });
            #endregion

            services.AddControllers(options =>
            {
                options.Filters.Add<HttpGlobalExceptionFilter>();
                options.EnableEndpointRouting = true;
            });

            services.AddLogging(logging =>
            {
                logging.AddConsole();
                logging.AddDebug();
            });

            //services.AddConsulConsumer();
            // AddPolly():熔断，参见QoSOptions配置
            services.AddOcelot().AddConsul().AddPolly();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment hostEnvironment)
        {
            if (hostEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseHsts();
            //app.UseHttpsRedirection();
            app.UseOcelot().Wait();

            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();//.RequireCors("AllowAll");
                endpoints.MapControllerRoute("default", "api/{controller=Home}/{action=Index}");
            });
        }

    }
}