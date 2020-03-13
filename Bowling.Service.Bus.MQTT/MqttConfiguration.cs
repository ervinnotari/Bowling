using Bowling.Infra.Utilities;
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
        private IConfiguration _configuration;
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
                if (string.IsNullOrEmpty(val)) val = "bowling/play";
                return val;
            }
        }
        public int Port
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsTopic);
                if (string.IsNullOrEmpty(val)) val = ConfigureHelper.Configuration["Port"];
                if (string.IsNullOrEmpty(val)) val = "1883";
                return int.Parse(val);
            }
        }

        internal bool IsEnabled() => Host != null;
    }
}