using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using OrchardCore.Logging;

namespace Tubumu.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var hostingConfig = new ConfigurationBuilder()
                .AddJsonFile("Hosting.json", optional: true)
                .Build();

            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(hostingConfig)
                .UseNLogWeb()
                .UseStartup<Startup>();

            return webHostBuilder;
        }
    }
}
