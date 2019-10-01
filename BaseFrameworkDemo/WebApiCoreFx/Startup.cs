using Alachisoft.NCache.Web.SessionState;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DBModel.Entity;
using IdentityModel;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using WebApiCoreFx.Filter;
using WebApiCoreFx.Injection;

namespace WebApiCoreFx
{
    public class Startup
    {
        public static SymmetricSecurityKey symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("need_to_get_this_from_enviroment"));

        public static ILoggerRepository repository { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            db_cdzContext.DbConnStr = Configuration["AppSetting:DbConnStr"];
            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("Log4net.config"));
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// 通过Autofac 实现Ioc
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters()
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role,
                        ValidIssuer = "YFAPICommomCore",
                        ValidAudience = "api",
                        IssuerSigningKey = symmetricKey
                    };
                });

            services.AddMvc(options =>
                {
                    options.Filters.Add<HttpGlobalExceptionFilter>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSession();
            // 配置启用NCache
            services.AddNCacheSession(Configuration.GetSection("NCacheSessions")); // 等效于下一行
            //services.AddNCacheSession(configuration =>
            //{
            //    configuration.CacheName = "mySessionCache";
            //    configuration.EnableLogs = true;
            //    configuration.SessionAppId = "NCacheSessionApp";
            //    configuration.SessionOptions.IdleTimeout = 5;
            //    configuration.SessionOptions.CookieName = "AspNetCore.Session";
            //});

            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("Version1", new Swashbuckle.AspNetCore.Swagger.Info()
                {
                    Version = "Version1",
                    Title = "Asp.Net Core WebAPI HelpPage",
                    TermsOfService = "None",
                    Contact = new Swashbuckle.AspNetCore.Swagger.Contact()
                    {
                        Name = "",
                        Email = "",
                        Url = ""
                    },
                    License = new Swashbuckle.AspNetCore.Swagger.License()
                    {
                        Name = "",
                        Url = ""
                    }
                });
                option.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WebApiCoreFx.xml"));
            });

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new Evolution());
            Assembly Service = Assembly.Load("LogicLayer");
            Assembly IService = Assembly.Load("ILogicLayer");
            builder.RegisterAssemblyTypes(IService, Service).AsImplementedInterfaces().Where(t => t.Name.EndsWith("Service"));
            //builder.RegisterAssemblyTypes(typeof(Startup).Assembly).AsImplementedInterfaces();
            var Container = builder.Build();
            return new AutofacServiceProvider(Container);
        }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        //    services.AddTransient<db_cdzContext>();
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            // 放在useMvc前，否则报错
            app.UseSession();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            //添加访问静态文件
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(AppDomain.CurrentDomain.BaseDirectory),
                RequestPath = @"/StaticFiles"
            });
            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/Version1/swagger.json", "Version1");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=myApp}/{action=Index}/{id?}");
            });
            //app.UseStaticFiles(); //使用静态文件

            // 设置文件上传保存路径
            string fileUploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UploadFiles");
            if (!Directory.Exists(fileUploadPath))
            {
                Directory.CreateDirectory(fileUploadPath);
            }
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(fileUploadPath),
                RequestPath = "/UploadFiles"
            });
        }
    }
}