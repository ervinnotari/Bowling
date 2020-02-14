using BowlingPainelOnBlazor.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BowlingPainelOnBlazor.Utils
{
    public static class StartupUtils
    {
        public static void AddAllApplicationServices(this IServiceCollection services)
        {
            AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => typeof(IBowlingService).IsAssignableFrom(p)).ToList().ForEach(clazz =>
            {
                if (!clazz.IsInterface) services.AddSingleton(clazz);
            });
        }

        public static void AddAllApplicationOptions(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<NMSConfigurations>(Configuration.GetSection(nameof(NMSConfigurations)));
        }
    }
}
