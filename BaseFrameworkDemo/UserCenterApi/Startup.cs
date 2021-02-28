using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ocelot.ConsulExtensions;
using Ocelot.ConsulExtensions.Model;
using System.Text;
using UserCenterApi.RouteExtensions;

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
            //services.AddConsulRegister();
            #endregion

            #region swagger文档
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My API",
                    Version = "v1",
                    Description = "A simple example ASP.NET Core Web API",
                    //TermsOfService = new Uri("https://example.com/terms"),
                    //Contact = new OpenApiContact
                    //{
                    //    Name = "Shayne Boyer",
                    //    Email = string.Empty,
                    //    //Url = new Uri("https://twitter.com/spboyer"),
                    //},
                    //License = new OpenApiLicense
                    //{
                    //    Name = "Use under LICX",
                    //    //Url = new Uri("https://example.com/license"),
                    //}
                });
                //option.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebApiCoreFx.xml"));
            });
            #endregion

            services.AddControllers(options =>
            {
                options.UseCentralRoutePrefix(new RouteAttribute("/UserService"));
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

            #region swagger文档
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            #endregion

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();//.RequireCors("AllowAll");
                endpoints.MapControllerRoute("default", "api/{controller=Home}/{action=Index}");
            });

            app.UseHealthChecks(serviceOptions.Value.HealthCheck);
            //app.UseConsul();
        }

    }
}