using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Bowling.Application.PainelOnBlazor
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = (args != null) ? Host.CreateDefaultBuilder(args) : Host.CreateDefaultBuilder();
            host.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .Build()
                .Run();
        }
    }
}