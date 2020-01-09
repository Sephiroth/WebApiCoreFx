using AopDLL;
using AspectCore.Configuration;
using AspectCore.Extensions.Autofac;
using AspectCore.Extensions.DependencyInjection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DBLayer.DAL;
using DBModel.Entity;
using IDBLayer.Interface;
using IdentityModel;
using ILogicLayer.Interface;
using log4net;
using log4net.Config;
using log4net.Repository;
using LogicLayer.Service;
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
using System.Text;
using System.Threading.Tasks;
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
        /// <returns>IServiceProvider</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region .net core ioc注册
            //services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            //services.AddTransient<IUserService, UserService>();
            #endregion

            #region 身份验证
            _ = services.AddAuthentication(s =>
            {
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
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
            #endregion

            services.AddMvc(options =>
                {
                    options.Filters.Add<HttpGlobalExceptionFilter>(); // 异常过滤器
                    options.EnableEndpointRouting = false;//default true
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSession();

            #region swagger文档
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
            #endregion

            #region Autofac注册程序集(自动注入)
            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(services);
            builder.RegisterModule(new Evolution());

            #region 基于AspectCore实现aop
            builder.RegisterDynamicProxy(null, config =>
            {
                // namespace1命名空间下的Service不会被代理
                //config.NonAspectPredicates.Add(Predicates.ForNameSpace("namespace1"));//"*.namespace1"
                // *Service结尾的Service不会被代理
                //config.NonAspectPredicates.Add(Predicates.ForService("*Service"));
                // *method结尾的Service不会被代理
                //config.NonAspectPredicates.Add(Predicates.ForMethod("*method"));
                config.Interceptors.AddTyped<DothingAfterInterceptorAttribute>(Predicates.ForService("*Service"));
                config.Interceptors.AddTyped<DothingBeforeInterceptorAttribute>(Predicates.ForService("*Service"));
                config.ThrowAspectException = true;
            });
            #endregion

            IContainer container = builder.Build();
            return new AutofacServiceProvider(container);
            #endregion
        }

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
            app.UseSwagger();

            //添加访问静态文件
            //app.UseStaticFiles();
            string folder = CreateDirectory("FileFolder");
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(folder) //
                //RequestPath = folder // 静态文件请求路径
            });

            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/Version1/swagger.json", "Version1");
            });
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="directory"></param>
        private string CreateDirectory(string directory)
        {
            DirectoryInfo info = null;
            if (Path.IsPathRooted(directory)) // 判断是否是绝对路径
            {
                directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directory);
            }
            if (!Directory.Exists(directory))
            {
                info = Directory.CreateDirectory(directory);
            }
            return info?.FullName;
        }

    }
}