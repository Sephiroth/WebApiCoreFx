using Autofac;
using Autofac.Extensions.DependencyInjection;
using DBModel.Entity;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using WebApiCoreFx.Filter;
using WebApiCoreFx.Injection;

namespace WebApiCoreFx
{
    public class Startup
    {
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
            services.AddMvc(options =>
                {
                    options.Filters.Add<HttpGlobalExceptionFilter>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSession();

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            // 放在useMvc前，否则报错
            app.UseSession();
            app.UseMvcWithDefaultRoute();
            app.UseMvc();
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