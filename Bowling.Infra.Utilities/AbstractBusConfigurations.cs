using System;
using System.Text.RegularExpressions;
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

        public Uri BrokerUri
        {
            get
            {
                string val = Environment.GetEnvironmentVariable(nameof(BrokerUri).ToUpper());
                if (string.IsNullOrEmpty(val)) val = Configuration[nameof(BrokerUri)];
                if (string.IsNullOrEmpty(val)) return null;
                UriBuilder builder = new UriBuilder(val);
                builder.Path = $"topic/{TopicMatcher(builder.Uri)}";
                return builder.Uri;
            }
        }

        public string Topic
        {
            get
            {
                return TopicMatcher(BrokerUri);
            }
        }

        public static string TopicMatcher(Uri uri)
        {
            string pattern = @"/(?:topic/)?(?<topic>(?:\w+/\w*)+)$";
            var mt = Regex.Match(uri.LocalPath, pattern, RegexOptions.IgnoreCase);
            if (!mt.Success) return DefaultTopic;
            mt.Groups.TryGetValue("topic", out Group e);
            return e.Value;
        }

    }
}
