using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Ocelot.GatewayProj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHostBuilder builder = new WebHostBuilder();
            builder.UseKestrel()
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   //.ConfigureAppConfiguration((hostingContext, config) =>
                   //{
                   //    config
                   //        .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                   //        .AddJsonFile("appsettings.json", true, true)
                   //        .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
                   //        .AddJsonFile("ocelot.json")
                   //        .AddJsonFile("NLog.config")
                   //        .AddEnvironmentVariables();
                   //})
                   //.ConfigureServices(s =>
                   //{
                   //    s.AddOcelot();
                   //})
                   //.ConfigureLogging(logging =>
                   //{
                   //    logging.ClearProviders();
                   //    logging.AddConsole();
                   //})
                   .UseStartup<Startup>()
                   .UseUrls("http://localhost:9000")
                   .Build()
                   .Run();
        }
    }
}