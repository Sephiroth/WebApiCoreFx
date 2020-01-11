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
            .ConfigureKestrel((context, options) =>
            {
                options.Limits.MaxRequestBodySize = null;//5242880;//设置应用服务器Kestrel请求体最大为5MB
            })
            .UseStartup<Startup>();
        //.UseKestrel(option=> { option.ListenAnyIP(10010); });
    }
}
