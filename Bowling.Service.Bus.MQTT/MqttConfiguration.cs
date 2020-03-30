using Microsoft.Extensions.Configuration;
using System;
using Bowling.Infra.Utilities;

namespace Bowling.Service.Bus.MQTT
{
    public class MqttConfiguration : AbstractBusConfigurations
    {

        public MqttConfiguration(IConfiguration configuration):base(configuration)
        {
        }

        public string Host
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(nameof(Host).ToUpper());
                if (string.IsNullOrEmpty(val)) val = Configuration[nameof(Host)];
                return val;
            }
        }

        public int Port
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(nameof(Port).ToUpper());
                if (string.IsNullOrEmpty(val)) val = Configuration[nameof(Port)];
                return int.Parse(val ?? "1883");
            }
        }

        public override bool IsEnabled() => Host != null;
    }
}