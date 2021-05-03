using System;
using Microsoft.Extensions.Configuration;

namespace Bowling.Infra.Utilities
{
    public abstract class AbstractBusConfigurations
    {
        public static string DefaultTopic { get; set; } = "bowling/play";
        protected readonly IConfiguration Configuration;

        protected AbstractBusConfigurations(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string Host
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(nameof(Host).ToUpper());
                if (string.IsNullOrEmpty(val)) val = Configuration[nameof(Host)];
                return val;
            }
        }

        public int Port
        {
            get
            {
                //var val = Environment.GetEnvironmentVariable(nameof(Port).ToUpper());
                //if (string.IsNullOrEmpty(val)) val = Configuration[nameof(Port)];
                return 1883;//int.Parse(val ?? "1883");
            }
        }

        public string BusUsername
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(nameof(BusUsername).ToUpper());
                if (string.IsNullOrEmpty(val)) val = Configuration["BusUsername"];
                return val;
            }
        }

        public string Password
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(nameof(Password).ToUpper());
                if (string.IsNullOrEmpty(val)) val = Configuration[nameof(Password)];
                return val;
            }
        }

        public string Topic
        {
            get
            {
                var val = Environment.GetEnvironmentVariable(nameof(Topic).ToUpper());
                if (string.IsNullOrEmpty(val)) val = Configuration[nameof(Topic)];
                return val ?? DefaultTopic;
            }
        }

        public abstract bool IsEnabled();
    }
}
