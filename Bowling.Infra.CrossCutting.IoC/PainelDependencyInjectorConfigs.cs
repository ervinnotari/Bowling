using Bowling.Domain.Game.Interfaces;
using Bowling.Service.Bus.MQTT;
using Bowling.Service.Game;
using Microsoft.Extensions.Configuration;
// using Bowling.Service.Bus.NMS;
using Microsoft.Extensions.DependencyInjection;

namespace Bowling.Infra.CrossCutting.IoC
{
    public static class PainelDependencyInjectorConfigs
    {
        public static void AddAllApplicationServices(this IServiceCollection services, IConfiguration confgs)
        {
            services.AddSingleton(confgs);
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IBusService, MqttService>();
            //services.AddTransient<IBusService, NmsService>();
        }
    }
}