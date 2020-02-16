using Apache.NMS;
using System;
using Apache.NMS.ActiveMQ;

namespace Bowling.Application.PlayWorkerService
{
    public class NMSConfigurations
    {
        public Uri Uri { get; set; }
        public string Username { get; set; } = Environment.GetEnvironmentVariable("NMSCONFIGURATIONS_USERNAME");
        public string Password { get; set; } = Environment.GetEnvironmentVariable("NMSCONFIGURATIONS_PASSWORD");
        public string Topic { get; set; } = Environment.GetEnvironmentVariable("NMSCONFIGURATIONS_TOPIC") ?? "Bowling.play.Topic";

        internal bool IsEnabled() => Uri != null;
        public NMSConfigurations()
        {
            var uriEnv = Environment.GetEnvironmentVariable("NMSCONFIGURATIONS_URI");
            if(uriEnv != null) Uri = new Uri(uriEnv);
        }
        internal IConnection GetConnection()
        {
            var factory = new ConnectionFactory(Uri);
            if (Username != null && Password != null)
                return factory.CreateConnection(Username, Password);
            return factory.CreateConnection();
        }

    }
}
