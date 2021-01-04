using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Ocelot.ConsulExtensions;
using Ocelot.ConsulExtensions.Model;
using System.Text;

namespace UserCenterApi
{
    public class Startup
    {
        public static SymmetricSecurityKey symmetricKey;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("key"));
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 注册服务到Consul
            //services.AddConsulConsumer();
            services.AddHealthChecks(); //添加健康检查，.net core自带的
            services.AddConsulRegister();
            #endregion

            services.AddControllers(options =>
            {
                options.EnableEndpointRouting = true;//default true
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<ConsulConfig> serviceOptions)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();//.RequireCors("AllowAll");
                endpoints.MapControllerRoute("default", "api/{controller=Home}/{action=Index}");
            });

            app.UseHealthChecks(serviceOptions.Value.HealthCheck);
            app.UseConsul();
        }

    }
}