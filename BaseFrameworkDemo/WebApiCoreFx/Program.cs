using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace WebApiCoreFx
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args)
        {
            string currentDirector = Directory.GetCurrentDirectory();

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(currentDirector)
                .AddJsonFile("appsettings.json", true)
                .Build();

            string[] urls = config.GetSection("StartUpUrls").Get<string[]>();
            IHostBuilder host = Host.CreateDefaultBuilder(args)
                //.UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel()
                    .UseUrls(urls)
                    .UseContentRoot(currentDirector)
                    .ConfigureKestrel(serverOptions =>
                    {
                        serverOptions.AllowSynchronousIO = true;/*启用同步IO*/
                    })
                    //.UseIISIntegration()
                    .UseStartup<Startup>()
                    .ConfigureLogging((hostingContext, builder) =>
                    {
                        builder.ClearProviders();
                        builder.SetMinimumLevel(LogLevel.Trace);
                        builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                        builder.AddConsole();
                        builder.AddDebug();
                    });
                });

            return host;
        }
    }
}
