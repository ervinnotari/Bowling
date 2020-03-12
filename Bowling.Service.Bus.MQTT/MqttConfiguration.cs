using System;
using System.Collections.Generic;
using System.Text;

namespace Bowling.Service.Bus.MQTT
{
    public static class MqttConfiguration
    {
        private const string BusConfigurationsUsername = "BUS_CONFIGURATIONS_USERNAME";
        private const string BusConfigurationsPassword = "BUS_CONFIGURATIONS_PASSWORD";
        private const string BusConfigurationsTopic = "BUS_CONFIGURATIONS_TOPIC";
        private const string BusConfigurationsUrl = "BUS_CONFIGURATIONS_URL";

        public static string Url
        {
            get
            {
                var val = ConfigureHelper.Configuration["Url"];
                if (string.IsNullOrEmpty(val)) val = Environment.GetEnvironmentVariable(BusConfigurationsUrl);
                return val;
            }
        }

        public static string Username
        {
            get
            {
                var val = ConfigureHelper.Configuration["Username"];
                if (string.IsNullOrEmpty(val)) val = Environment.GetEnvironmentVariable(BusConfigurationsUsername);
                return val;
            }
        }

        public static string Password
        {
            get
            {
                var val = ConfigureHelper.Configuration["Username"];
                if (string.IsNullOrEmpty(val)) val = Environment.GetEnvironmentVariable(BusConfigurationsPassword);
                return val;
            }
        }

        public static string Topic
        {
            get
            {
                var val = ConfigureHelper.Configuration["Username"];
                if (string.IsNullOrEmpty(val)) val = Environment.GetEnvironmentVariable(BusConfigurationsTopic);
                if (string.IsNullOrEmpty(val)) val = "bowling/play";
                return val;
            }
        }
        public static int Port
        {
            get
            {
                var val = ConfigureHelper.Configuration["Port"];
                if (string.IsNullOrEmpty(val)) val = Environment.GetEnvironmentVariable(BusConfigurationsTopic);
                if (string.IsNullOrEmpty(val)) val = "1883";
                return int.Parse(val);
            }
        }

        internal static bool IsEnabled() => Url != null;
    }
}