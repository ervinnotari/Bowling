using Bowling.Domain.Game.Interfaces;
using Bowling.Domain.Game.Utils;
using Bowling.Service;
using Bowling.Service.Bus.MQTT;
using Microsoft.Extensions.DependencyInjection;

namespace Bowling.Infra.CrossCuting.IoC
{
    public static class PainelDependencyInjectorConfigs
    {
        public static void AddAllApplicationServices(this IServiceCollection services)
        {
            AddAllApplicationServices(services, null);
        }

        public static void AddAllApplicationServices(this IServiceCollection services, BusConfiguration busConfiguration)
        {
            if (busConfiguration == null) services.AddSingleton<BusConfiguration>();
            else services.AddSingleton<BusConfiguration>(busConfiguration);
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IBusService, MqttService>();
        }
    }
}
