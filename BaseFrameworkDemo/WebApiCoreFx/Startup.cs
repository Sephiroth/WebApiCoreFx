using Autofac;
using CustomizeMiddleware;
using DBLayer.DAL;
using DBModel.Entity;
using EFCoreDBLayer.DAL;
using IDBLayer.Interface;
using IdentityModel;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WebApiCoreFx.Filter;

namespace WebApiCoreFx
{
    public class Startup
    {
        private readonly SymmetricSecurityKey symmetricKey;
        public static readonly LoggerFactory loggerFactory = new LoggerFactory();
        public static ILoggerRepository LogRep { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            db_cdzContext.DbConnStr = Configuration.GetConnectionString("MysqlConnection");
            LogRep = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(LogRep, new FileInfo("Log4net.config"));
            symmetricKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetSection("Authentication").GetValue<string>("SymmetricKey")));
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// (.net core2.2及以下,通过Autofac 实现Ioc,返回IServiceProvider)
        /// </summary>
        /// <param name="services"></param>
        /// <returns>IServiceProvider</returns>
        public void ConfigureServices(IServiceCollection services) // IServiceProvider
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
                options.Filters.Add<LogicLayer.Attribute.CustomizeAuthorizationFilter>();
                options.EnableEndpointRouting = true;//default true
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }); // .AddControllersAsServices();使用属性注入而不是构造函数注入，必须加AddControllersAsServices

            #region .net core ioc注册
            services.AddTransient(typeof(IRepository<>), typeof(DbRepository<>));
            //services.AddTransient<IUserService, UserService>();
            // 注册单例ArrayPool<T>
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton(s=> {
                var provider = s.GetRequiredService<ObjectPoolProvider>();
                return provider.Create<object>();
            });
            #endregion

            #region CSRF
            services.AddAntiforgery(option =>
            {
                option.Cookie.Name = "CSRF-TOKEN";
                option.Cookie.SameSite = SameSiteMode.Lax; ;
                option.FormFieldName = "CustomerFieldName";
                option.HeaderName = "X-XSRF-TOKEN";//Request.Header包含X-XSRF-TOKEN才能正常请求
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
                            context.Token = context.Request.Headers["token"];
                            return Task.CompletedTask;
                        }
                    };
                });
            #endregion

            #region 限制上传文件
            services.Configure<FormOptions>(options =>
            {
                // 限制上传文件的大小
                //options.MultipartBodyLengthLimit = int.MaxValue;
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

            services.AddHttpClient();
            services.AddSession();

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

            #region Autofac IOC注册程序集(.Net Core 2.2及以下,3.0以上废弃)
            //ContainerBuilder builder = new ContainerBuilder();
            //builder.Populate(services);
            //builder.RegisterModule(new Evolution());

            //#region 基于AspectCore实现aop
            //builder.RegisterDynamicProxy(null, config =>
            //{
            //    // namespace1命名空间下的Service不会被代理
            //    //config.NonAspectPredicates.Add(Predicates.ForNameSpace("namespace1"));//"*.namespace1"
            //    // *Service结尾的Service不会被代理
            //    //config.NonAspectPredicates.Add(Predicates.ForService("*Service"));
            //    // *method结尾的Service不会被代理
            //    //config.NonAspectPredicates.Add(Predicates.ForMethod("*method"));
            //    config.Interceptors.AddTyped<DothingAfterInterceptorAttribute>(Predicates.ForService("*Service"));
            //    config.Interceptors.AddTyped<DothingBeforeInterceptorAttribute>(Predicates.ForService("*Service"));
            //    config.Interceptors.AddTyped<DothingBeforeInterceptorAttribute>(Predicates.ForMethod("Get*"));
            //    //config.Interceptors.AddTyped<DothingBeforeInterceptorAttribute>(Predicates.ForNameSpace("WebApiCoreFx.Controllers"));
            //    config.ThrowAspectException = true;
            //});
            //#endregion

            //IContainer container = builder.Build();
            //return new AutofacServiceProvider(container);
            #endregion
        }

        /// <summary>
        /// .Net Core 3.0及以上,通过Autofac ioc注册程序集
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder)
        {
            #region 获取所有控制器类型并使用属性注入
            //var controllerBaseType = typeof(ControllerBase);
            //builder.RegisterAssemblyTypes(typeof(Program).Assembly)
            //    .Where(t => controllerBaseType.IsAssignableFrom(t) && t != controllerBaseType)
            //    .PropertiesAutowired();
            #endregion

            #region 
            //业务逻辑实现
            Assembly serviceImpl = Assembly.Load("LogicLayer");
            //业务逻辑接口
            Assembly iService = Assembly.Load("ILogicLayer");
            //自动注入
            builder.RegisterAssemblyTypes(serviceImpl, iService).Where(t => t.Name.EndsWith("Service")).AsImplementedInterfaces();
            //注册仓储，所有IRepository接口到Repository的映射
            //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            #endregion

            #region 基于AspectCore实现aop
            //builder.RegisterDynamicProxy(null, config =>
            //{
            //    // namespace1命名空间下的Service不会被代理
            //    //config.NonAspectPredicates.Add(Predicates.ForNameSpace("namespace1"));//"*.namespace1"
            //    // *Service结尾的Service不会被代理
            //    //config.NonAspectPredicates.Add(Predicates.ForService("*Service"));
            //    // *method结尾的Service不会被代理
            //    //config.NonAspectPredicates.Add(Predicates.ForMethod("*method"));
            //    config.Interceptors.AddTyped<DothingAfterInterceptorAttribute>(Predicates.ForService("*Service"));
            //    config.Interceptors.AddTyped<DothingBeforeInterceptorAttribute>(Predicates.ForService("*Service"));
            //    config.Interceptors.AddTyped<DothingBeforeInterceptorAttribute>(Predicates.ForMethod("Get*"));
            //    //config.Interceptors.AddTyped<DothingBeforeInterceptorAttribute>(Predicates.ForNameSpace("WebApiCoreFx.Controllers"));
            //    config.ThrowAspectException = true;
            //});
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddProvider(new Log4NetProvider("Log4net.config"));
            //loggerFactory.AddLog4Net("Log4net.config");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            #region swagger文档
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            #endregion

            // 强制使用https重定向
            app.UseHttpsRedirection();

            //添加访问静态文件
            string folder = CreateDirectory("FileFolder");
            //app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(folder) //
                //RequestPath = folder // 静态文件请求路径
            });

            app.UseRouting();
            app.UseAuthentication();//认证
            app.UseAuthorization();
            // 放在useMvc前，否则报错
            // app.UseSession();

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
            // 指定路径,多层次匹配
            //app.Map("/api", level1Builder =>
            //{
            //    level1Builder.Map("/Values", level2Builder =>
            //    {
            //        level2Builder.Run(async context =>
            //        {
            //            await context.Response.WriteAsync("app.Map指定/api/Values");
            //        });
            //    });
            //});
            // 指定满足条件时触发
            //app.MapWhen(context => context.Request.Headers.ContainsKey("branch"), HandleBranch);
            #endregion

            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();//.RequireCors("AllowAll");
                endpoints.MapControllerRoute("default", "api/{controller=Home}/{action=Index}");
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

        private void HandleBranch(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                bool branchVer = context.Request.Headers.ContainsKey("branch");
                await context.Response.WriteAsync($"Branch used = {branchVer}");
            });
        }

    }
}