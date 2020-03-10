using System;
using System.Collections.Generic;
using System.Text;

namespace Bowling.Domain.Game.Utils
{
    public abstract class BusConfiguration
    {
        public const string BUS_CONFIGURATIONS_USERNAME = "NMSCONFIGURATIONS_USERNAME";
        public const string BUS_CONFIGURATIONS_PASSWORD = "NMSCONFIGURATIONS_PASSWORD";
        public const string BUS_CONFIGURATIONS_TOPIC = "NMSCONFIGURATIONS_TOPIC";
        public const string BUS_CONFIGURATIONS_URI = "NMSCONFIGURATIONS_URI";

        public Uri Uri { get; set; }
        public string Username { get; set; } = Environment.GetEnvironmentVariable(BUS_CONFIGURATIONS_USERNAME);
        public string Password { get; set; } = Environment.GetEnvironmentVariable(BUS_CONFIGURATIONS_PASSWORD);
        public string Topic { get; set; } = Environment.GetEnvironmentVariable(BUS_CONFIGURATIONS_TOPIC) ?? "Bowling/play";

        internal virtual bool IsEnabled() => Uri != null;

        public BusConfiguration()
        {
            var uriEnv = Environment.GetEnvironmentVariable(BUS_CONFIGURATIONS_URI);
            if (uriEnv != null) Uri = new Uri(uriEnv);
        }

        public BusConfiguration(Uri uri, string username, string password, string topic)
        {
            Uri = uri;
            Username = username;
            Password = password;
            Topic = topic;
        }
    }
}
