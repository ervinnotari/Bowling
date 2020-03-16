using Microsoft.Extensions.Configuration;
using System;

namespace Bowling.Service.Bus.NMS
{
    public class NmsConfiguration
    {
        private const string BusConfigurationsUsername = "NMS_USERNAME";
        private const string BusConfigurationsPassword = "NMS_PASSWORD";
        private const string BusConfigurationsTopic = "NMS_TOPIC";
        private const string BusConfigurationsUri = "NMS_URL";
        private readonly IConfiguration _configuration;
        public NmsConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Uri Uri
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(BusConfigurationsUri);
                if (string.IsNullOrEmpty(val)) val = _configuration["Url"];
                return val == null ? null : new Uri(val);
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

        public bool IsEnabled() => Uri != null;
    }
}