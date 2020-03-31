using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Bowling.Application.PainelOnBlazor
{
    public static class Program
    {
        public static void Main()
        {
            var args = new string[] { };
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
