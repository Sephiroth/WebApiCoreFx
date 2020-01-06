using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebApiCoreFx
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //.UseUrls("http://*:10010")
            .UseStartup<Startup>();
            //.UseKestrel(option=> { option.ListenAnyIP(10010); });
    }
}
