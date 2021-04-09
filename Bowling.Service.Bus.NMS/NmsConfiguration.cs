using Microsoft.Extensions.Configuration;
using System;
using Bowling.Infra.Utilities;

namespace Bowling.Service.Bus.NMS
{
    public class NmsConfiguration : AbstractBusConfigurations
    {
        public NmsConfiguration(IConfiguration configuration):base(configuration)
        {
        }

        public Uri Uri
        {
            get
            {
                var val = Environment.GetEnvironmentVariable("URL");
                if (string.IsNullOrEmpty(val)) val = Configuration["Url"];
                return string.IsNullOrEmpty(val) ? null : new Uri(val);
            }
        }

        public override bool IsEnabled() => Uri != null;
    }
}