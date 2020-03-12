using System;
using Microsoft.Extensions.Configuration;

// ReSharper disable All

namespace Bowling.Service.Bus.MQTT
{
    public class ConfigureHelper
    {
        static ConfigureHelper()
        {
            try
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(ProcessDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
            }
            catch (Exception)
            {
                Configuration = new ConfigurationBuilder()
                    .SetBasePath(ProcessDirectory)
                    .AddInMemoryCollection()
                    .Build();
            }
        }

        public static string ProcessDirectory
        {
            get
            {
#if NETSTANDARD1_3
                return AppContext.BaseDirectory;
#else
                return AppDomain.CurrentDomain.BaseDirectory;
#endif
            }
        }

        public static IConfigurationRoot Configuration { get; }
    }
}