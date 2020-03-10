using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bowling.Application.PlayWorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<NMSBowlingService>();
                    services.Configure<NMSConfigurations>(hostContext.Configuration.GetSection(nameof(NMSConfigurations)));
                });
    }
}
