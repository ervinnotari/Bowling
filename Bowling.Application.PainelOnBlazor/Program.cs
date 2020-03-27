using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Bowling.Application.PainelOnBlazor
{
    public static class Program
    {
        protected static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
