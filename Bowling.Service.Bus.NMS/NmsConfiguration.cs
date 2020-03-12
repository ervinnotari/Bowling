using System;
using System.Collections.Generic;
using System.Text;
using Bowling.Infra.Utilities;

namespace Bowling.Service.Bus.NMS
{
    public static class NmsConfiguration
    {
        private const string BusConfigurationsUsername = "NMS_USERNAME";
        private const string BusConfigurationsPassword = "NMS_PASSWORD";
        private const string BusConfigurationsTopic = "NMS_TOPIC";
        private const string BusConfigurationsUri = "NMS_URL";

        public static Uri Uri
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsUri);
                if (string.IsNullOrEmpty(val)) val = ConfigureHelper.Configuration["Url"];
                return new Uri(val);
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

        internal static bool IsEnabled() => Uri != null;
    }
}