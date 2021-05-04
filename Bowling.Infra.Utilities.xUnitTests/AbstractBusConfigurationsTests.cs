using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Xunit;

namespace Bowling.Infra.Utilities.xUnitTests
{
    public class AbstractBusConfigurationsTests
    {
        private const string URI_A = "tcp://user:pass@broker.mqttdashboard.com:1883/topic/bowling/MQTT_xUnitTests";
        private const string URI_B = "tcp://user:pass@broker.mqttdashboard.com:1883/topic/bowling/play";

        [Theory]
        [InlineData(URI_A, URI_A)]
        [InlineData("tcp://user:pass@broker.mqttdashboard.com:1883/", URI_B)]
        [InlineData("tcp://user:pass@broker.mqttdashboard.com:1883", URI_B)]
        public void ValuesTest(string uri, string result)
        {
            IConfiguration _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { "BrokerUri", uri } })
                .Build();
            var t = new TestBusConfigurationsClass(_configuration);
            Assert.Equal(t.BrokerUri.AbsoluteUri, result);
            Assert.Equal(t.Topic, TestBusConfigurationsClass.TopicMatcher(new Uri(uri)));
        }

        [Fact]
        public void ValuesNullTest()
        {
            IConfiguration _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { })
                .Build();
            var t = new TestBusConfigurationsClass(_configuration);
            Environment.SetEnvironmentVariable(nameof(t.BrokerUri), null);
            Assert.Null(Environment.GetEnvironmentVariable(nameof(t.BrokerUri)));
            Assert.Null(t.BrokerUri);
        }

        [Fact]
        public void DefaultTopicTest()
        {
            var str = "teste";
            AbstractBusConfigurations.DefaultTopic = str;
            Assert.Equal(AbstractBusConfigurations.DefaultTopic, str);
        }

        public class TestBusConfigurationsClass : AbstractBusConfigurations
        {
            public TestBusConfigurationsClass(IConfiguration configuration) : base(configuration) { }
        }
    }
}
