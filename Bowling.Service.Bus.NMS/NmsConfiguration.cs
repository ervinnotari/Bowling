using System;
using System.Collections.Generic;
using System.Text;

namespace Bowling.Service.Bus.NMS
{
    public class NmsConfiguration
    {
        public const string BUS_CONFIGURATIONS_USERNAME = "BUS_CONFIGURATIONS_USERNAME";
        public const string BUS_CONFIGURATIONS_PASSWORD = "BUS_CONFIGURATIONS_PASSWORD";
        public const string BUS_CONFIGURATIONS_TOPIC = "BUS_CONFIGURATIONS_TOPIC";
        public const string BUS_CONFIGURATIONS_URI = "BUS_CONFIGURATIONS_URI";

        public Uri Uri { get; set; }
        public string Username { get; set; } = Environment.GetEnvironmentVariable(BUS_CONFIGURATIONS_USERNAME);
        public string Password { get; set; } = Environment.GetEnvironmentVariable(BUS_CONFIGURATIONS_PASSWORD);
        public string Topic { get; set; } = Environment.GetEnvironmentVariable(BUS_CONFIGURATIONS_TOPIC) ?? "Bowling/play";

        internal virtual bool IsEnabled() => Uri != null;

        public NmsConfiguration()
        {
            var uriEnv = Environment.GetEnvironmentVariable(BUS_CONFIGURATIONS_URI);
            if (uriEnv != null) Uri = new Uri(uriEnv);
        }

        public NmsConfiguration(Uri uri, string username, string password, string topic)
        {
            Uri = uri;
            Username = username;
            Password = password;
            Topic = topic;
        }
    }
}
