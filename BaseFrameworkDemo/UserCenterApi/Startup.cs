using Consul;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;
using UserCenterApi.Extensions;
using UserCenterApi.Model;

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
            //从配置文件中获取ServiceDiscovery
            services.Configure<ServiceDisvoveryOptions>(Configuration.GetSection("ServiceDiscovery"));
            //单例注册ConsulClient
            services.AddSingleton<IConsulClient>(p => new ConsulClient(cfg =>
            {
                var serviceConfiguration = p.GetRequiredService<IOptions<ServiceDisvoveryOptions>>().Value;
                if (!string.IsNullOrEmpty(serviceConfiguration.Consul.HttpEndpoint))
                {
                    // if not configured, the client will use the default value "127.0.0.1:8500"
                    cfg.Address = new Uri(serviceConfiguration.Consul.HttpEndpoint);
                }
            }));

            _ = services.AddAuthentication(s =>
            {
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role,
                        ValidIssuer = "YFAPICommomCore",
                        ValidAudience = "api",
                        IssuerSigningKey = symmetricKey,
                        // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                        ValidateLifetime = true,
                        //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Query["AccessToken"];
                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddConsulConfig(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory LoggerFactory,
            IApplicationLifetime lifetime,
            IConsulClient consulClient,
            IOptions<ServiceDisvoveryOptions> DisvoveryOptions)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseConsul();
            app.UseMvc();
            app.UseAuthentication();

            lifetime.ApplicationStarted.Register(() =>
            {
                AppBuilderExtensions.RegisterConsul(app, Configuration, DisvoveryOptions, consulClient);
            });
            lifetime.ApplicationStopped.Register(() =>
            {
                AppBuilderExtensions.RemoveService(app, DisvoveryOptions, consulClient);
            });
        }
    }
}