using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace UserCenterApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls("http://10.1.72.24:5000"); //"https://10.1.72.24:5001",
                });
    }
}