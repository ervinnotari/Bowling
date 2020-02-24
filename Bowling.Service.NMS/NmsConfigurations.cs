using System;

namespace Bowling.Service.NMS
{
    public class NmsConfigurations
    {
        public const string NMSCONFIGURATIONS_USERNAME = "NMSCONFIGURATIONS_USERNAME";
        public const string NMSCONFIGURATIONS_PASSWORD = "NMSCONFIGURATIONS_PASSWORD";
        public const string NMSCONFIGURATIONS_TOPIC = "NMSCONFIGURATIONS_TOPIC";
        public const string NMSCONFIGURATIONS_URI = "NMSCONFIGURATIONS_URI";

        public Uri Uri { get; set; }
        public string Username { get; set; } = Environment.GetEnvironmentVariable(NMSCONFIGURATIONS_USERNAME);
        public string Password { get; set; } = Environment.GetEnvironmentVariable(NMSCONFIGURATIONS_PASSWORD);
        public string Topic { get; set; } = Environment.GetEnvironmentVariable(NMSCONFIGURATIONS_TOPIC) ?? "Bowling.play.Topic";

        internal bool IsEnabled() => Uri != null;
        public NmsConfigurations()
        {
            var uriEnv = Environment.GetEnvironmentVariable(NMSCONFIGURATIONS_URI);
            if (uriEnv != null) Uri = new Uri(uriEnv);
        }

        public NmsConfigurations(Uri uri, string username, string password, string topic)
        {
            Uri = uri;
            Username = username;
            Password = password;
            Topic = topic;
        }
    }

}
