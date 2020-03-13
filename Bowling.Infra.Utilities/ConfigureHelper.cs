using Microsoft.Extensions.Configuration;
using System;

// ReSharper disable All

namespace Bowling.Infra.Utilities
{
    public class ConfigureHelper
    {

        static ConfigureHelper()
        {

            // #if DEBUG
            //             string env = "Development";
            // #else
            //             string env = "Production";
            // #endif
            //             env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? env;
            //             Configuration = new ConfigurationBuilder()
            //                 .SetBasePath(ProcessDirectory)
            //                 .AddInMemoryCollection()
            //                 .AddJsonFile($"appsettings.{env}.json", optional: true,reloadOnChange: true)
            //                 .AddJsonFile("appsettings.json", optional: true,reloadOnChange: true)
            //                 .Build();
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