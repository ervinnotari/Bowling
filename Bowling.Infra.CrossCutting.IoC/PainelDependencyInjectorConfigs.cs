﻿using Bowling.Domain.Game.Interfaces;
using Bowling.Service;
using Bowling.Service.Bus.MQTT;
// using Bowling.Service.Bus.NMS;
using Microsoft.Extensions.DependencyInjection;

namespace Bowling.Infra.CrossCutting.IoC
{
    public static class PainelDependencyInjectorConfigs
    {
        public static void AddAllApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IBusService, MqttService>();
            //services.AddTransient<IBusService, NmsService>();
        }
    }
}