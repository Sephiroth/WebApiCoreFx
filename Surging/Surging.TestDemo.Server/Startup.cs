using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.EventBusRabbitMQ.Configurations;

namespace Surging.TestDemo.Server
{
    public class Startup
    {
        public Startup(IConfigurationBuilder config)
        {
            ConfigureEventBus(config);
            ConfigureCache(config);
        }

        public IContainer ConfigureServices(ContainerBuilder builder)
        {
            var services = new ServiceCollection();
            //services.AddAuthentication(s =>
            //{
            //    s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(o =>
            //{
            //    o.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        NameClaimType = JwtClaimTypes.Name,
            //        RoleClaimType = JwtClaimTypes.Role,
            //        ValidIssuer = "YFAPICommomCore",
            //        ValidAudience = "api",
            //        IssuerSigningKey = symmetricKey,
            //        // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
            //        ValidateLifetime = true,
            //        //注意这是缓冲过期时间，总的有效时间等于这个时间加上jwt的过期时间，如果不配置，默认是5分钟
            //        ClockSkew = TimeSpan.FromMinutes(5)
            //    };
            //    o.Events = new JwtBearerEvents()
            //    {
            //        OnMessageReceived = context =>
            //        {
            //            context.Token = context.Request.Query["AccessToken"];
            //            return Task.CompletedTask;
            //        }
            //    };
            //});
            ConfigureLogging(services);
            builder.Populate(services);
            ServiceLocator.Current = builder.Build();
            return ServiceLocator.Current;
        }

        public void Configure(IContainer app)
        {

        }

        #region 私有方法
        /// <summary>
        /// 配置日志服务
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureLogging(IServiceCollection services)
        {
            services.AddLogging();
        }

        private static void ConfigureEventBus(IConfigurationBuilder build)
        {
            build.AddEventBusFile("eventBusSettings.json", optional: false);
        }

        /// <summary>
        /// 配置缓存服务
        /// </summary>
        private void ConfigureCache(IConfigurationBuilder build)
        {
            //build.AddCacheFile("cacheSettings.json", optional: false);
        }
        #endregion
    }
}
