using Bowling.Domain.Game.Interfaces;
using Bowling.Domain.Game.Utils;
using Bowling.Service;
using Bowling.Service.NMS;
using Microsoft.Extensions.DependencyInjection;

namespace Bowling.Infra.CrossCuting.IoC
{
    public static class PainelDependencyInjectorConfigs
    {
        public static void AddAllApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IBusService, NmsService>();
            services.AddSingleton<BusConfiguration, NmsConfigurations>();
        }
    }
}
