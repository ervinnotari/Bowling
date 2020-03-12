using System;
using System.Collections.Generic;
using System.Text;
using Bowling.Infra.Utilities;

namespace Bowling.Service.Bus.MQTT
{
    public static class MqttConfiguration
    {
        private const string BusConfigurationsUsername = "MQTT_USERNAME";
        private const string BusConfigurationsPassword = "MQTT_PASSWORD";
        private const string BusConfigurationsTopic = "MQTT_TOPIC";
        private const string BusConfigurationsUrl = "MQTT_HOST";

        public static string Host
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsUrl);
                if (string.IsNullOrEmpty(val)) val = ConfigureHelper.Configuration["Host"];
                return val;
            }
        }

        public static string Username
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsUsername);
                if (string.IsNullOrEmpty(val)) val = ConfigureHelper.Configuration["Username"];
                return val;
            }
        }

        public static string Password
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsPassword);
                if (string.IsNullOrEmpty(val)) val = ConfigureHelper.Configuration["Password"];
                return val;
            }
        }

        public static string Topic
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsTopic);
                if (string.IsNullOrEmpty(val)) val = ConfigureHelper.Configuration["Topic"];
                if (string.IsNullOrEmpty(val)) val = "bowling/play";
                return val;
            }
        }
        public static int Port
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsTopic);
                if (string.IsNullOrEmpty(val)) val = ConfigureHelper.Configuration["Port"];
                if (string.IsNullOrEmpty(val)) val = "1883";
                return int.Parse(val);
            }
        }

        internal static bool IsEnabled() => Host != null;
    }
}