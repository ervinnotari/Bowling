using Microsoft.Extensions.Configuration;
using System;

namespace Bowling.Service.Bus.MQTT
{
    public class MqttConfiguration
    {
        private const string BusConfigurationsUsername = "MQTT_USERNAME";
        private const string BusConfigurationsPassword = "MQTT_PASSWORD";
        private const string BusConfigurationsTopic = "MQTT_TOPIC";
        private const string BusConfigurationsUrl = "MQTT_HOST";
        private readonly IConfiguration _configuration;

        public MqttConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Host
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsUrl);
                if (string.IsNullOrEmpty(val)) val = _configuration["Host"];
                return val;
            }
        }

        public string Username
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsUsername);
                if (string.IsNullOrEmpty(val)) val = _configuration["Username"];
                return val;
            }
        }

        public string Password
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsPassword);
                if (string.IsNullOrEmpty(val)) val = _configuration["Password"];
                return val;
            }
        }

        public string Topic
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsTopic);
                if (string.IsNullOrEmpty(val)) val = _configuration["Topic"];
                return val ?? "bowling/play";
            }
        }

        public int Port
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsTopic);
                if (string.IsNullOrEmpty(val)) val = _configuration["Port"];
                return int.Parse(val ?? "1883");
            }
        }

        public bool IsEnabled() => Host != null;
    }
}