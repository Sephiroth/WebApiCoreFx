using AopDLL;
using AspectCore.Configuration;
using AspectCore.Extensions.Autofac;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CustomizeMiddleware;
using DBModel.Entity;
using IdentityModel;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
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
        private SymmetricSecurityKey symmetricKey;
        public static readonly LoggerFactory loggerFactory = new LoggerFactory();
        public static ILoggerRepository repository { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            db_cdzContext.DbConnStr = Configuration.GetConnectionString("MysqlConnection");
            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("Log4net.config"));
            symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("Authentication").GetValue<string>("SymmetricKey")));
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

            #region CSRF
            services.AddAntiforgery(option =>
            {
                option.Cookie.Name = "CSRF-TOKEN";
                option.Cookie.SameSite = SameSiteMode.Lax; ;
                option.FormFieldName = "CustomerFieldName";
                option.HeaderName = "CSRF-TOKEN";
            });
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
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            #endregion

            #region 限制上传文件
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = long.MaxValue;
            });
            #endregion

            #region 添加缓存
            services.AddDistributedRedisCache(options =>
            {
                //用于连接Redis的配置  Configuration.GetConnectionString("RedisConnectionString")读取配置信息的串
                options.Configuration = Configuration["Redis:ConnectionString"];
                //Redis实例名RedisDistributedCache
                options.InstanceName = "RedisDistributedCache";
            });
            #endregion

            #region EFCore-Mysql的DbContextPool设置(poolSize要比数据库连接池小)
            services.AddDbContextPool<db_cdzContext>(dbCtxBuilder =>
            {
                dbCtxBuilder.UseMySql(Configuration.GetConnectionString("MysqlConnection"));
                dbCtxBuilder.UseLoggerFactory(loggerFactory);
            }, 8);
            #endregion

            #region 中间件
            // 如果自定义Middleware继承了IMiddleware接口,必须在此注册，否则报错
            services.AddSingleton<Test2Middleware>();
            #endregion

            services.AddMvc(options =>
                {
                    options.Filters.Add<HttpGlobalExceptionFilter>(); // 异常过滤器
                    options.Filters.Add<LogicLayer.Attribute.CustomizeAuthorizationFilter>();
                    options.EnableEndpointRouting = false;//default true
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
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

            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", p =>
                {
                    // 允许所有的域名请求 // 允许所有的请求方式GET/POST/PUT/DELETE // 允许所有的头部参数 // 允许携带Cookie
                    p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
                });
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
                config.Interceptors.AddTyped<DothingBeforeInterceptorAttribute>(Predicates.ForMethod("Get*"));
                //config.Interceptors.AddTyped<DothingBeforeInterceptorAttribute>(Predicates.ForNameSpace("WebApiCoreFx.Controllers"));
                config.ThrowAspectException = true;
            });
            #endregion

            IContainer container = builder.Build();
            return new AutofacServiceProvider(container);
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) // , ILoggerFactory loggerFactory
        {
            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // 强制使用https重定向
            // app.UseHttpsRedirection();

            // 放在useMvc前，否则报错
            // app.UseSession();
            app.UseAuthentication();

            #region swagger文档
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/Version1/swagger.json", "Version1");
            });
            #endregion

            //添加访问静态文件
            string folder = CreateDirectory("FileFolder");
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(folder) //
                //RequestPath = folder // 静态文件请求路径
            });

            // CORS 中间件必须配置为在对 UseRouting 和 UseEndpoints的调用之间执行
            app.UseCors("AllowAll");

            #region 使用自定义中间件
            app.UseMiddleware<CustomizeMiddleware.Test2Middleware>();
            app.UseTestMiddleware();
            // 匿名中间件
            //app.Use(async (context, next) =>
            //{
            //    // Do work that doesn't write to the Response.
            //    await next.Invoke();
            //    // Do logging or other work that doesn't write to the Response.
            //});
            // 指定路径
            //app.Map("/api/Values", builder =>
            //{
            //    builder.Run(async context =>
            //    {
            //        await context.Response.WriteAsync("app.Map指定/api/Values");
            //    });
            //});
            #endregion

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "api/{controller=Home}/{action=Index}");
            });
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="directory"></param>
        private string CreateDirectory(string directory)
        {
            DirectoryInfo info = null;
            if (!Path.IsPathRooted(directory)) // 判断是否是绝对路径
                directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directory);

            if (!Directory.Exists(directory))
                info = Directory.CreateDirectory(directory);
            return info?.FullName ?? directory;
        }

    }
}