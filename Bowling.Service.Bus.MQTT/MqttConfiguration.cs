using Microsoft.Extensions.Configuration;
using System;
using Bowling.Infra.Utilities;

namespace Bowling.Service.Bus.MQTT
{
    public class MqttConfiguration : AbstractBusConfigurations
    {

        public MqttConfiguration(IConfiguration configuration) : base(configuration) { }

        public override bool IsEnabled() => Host != null;
    }
}