using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Xunit;

namespace Bowling.Infra.Utilities.xUnitTests
{
    public class AbstractBusConfigurationsTests
    {

        private readonly string TOPIC = "bowling/xUnitTests";
        private readonly string BUSUSERNAME = "teste";
        private readonly string PASSWORD = "123456";

        [Fact]
        public void ValuesTest()
        {
            IConfiguration _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { { "Topic", TOPIC }, { "BusUsername", BUSUSERNAME }, { "Password", PASSWORD } })
                .Build();
            var t = new TestBusConfigurationsClass(_configuration);
            Assert.Equal(t.Topic, TOPIC);
            Assert.Equal(t.BusUsername, BUSUSERNAME);
            Assert.Equal(t.Password, PASSWORD);
            Environment.SetEnvironmentVariable(nameof(t.Topic), TOPIC + "2");
            Environment.SetEnvironmentVariable(nameof(t.BusUsername), BUSUSERNAME + "2");
            Environment.SetEnvironmentVariable(nameof(t.Password), PASSWORD + "2");
            Assert.Equal(t.Topic, TOPIC + "2");
            Assert.Equal(t.BusUsername, BUSUSERNAME + "2");
            Assert.Equal(t.Password, PASSWORD + "2");
        }

        [Fact]
        public void ValuesNullTest()
        {
            IConfiguration _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { })
                .Build();
            var t = new TestBusConfigurationsClass(_configuration);
            Environment.SetEnvironmentVariable(nameof(t.Topic), null);
            Environment.SetEnvironmentVariable(nameof(t.BusUsername), null);
            Environment.SetEnvironmentVariable(nameof(t.Password), null);
            Assert.Null(Environment.GetEnvironmentVariable(nameof(t.Topic)));
            Assert.Null(Environment.GetEnvironmentVariable(nameof(t.BusUsername)));
            Assert.Null(Environment.GetEnvironmentVariable(nameof(t.Password)));
            Assert.Equal(t.Topic, AbstractBusConfigurations.DefaultTopic);
            Assert.Null(t.BusUsername);
            Assert.Null(t.Password);
            Environment.SetEnvironmentVariable(nameof(t.Topic), TOPIC);
            Environment.SetEnvironmentVariable(nameof(t.BusUsername), BUSUSERNAME);
            Environment.SetEnvironmentVariable(nameof(t.Password), PASSWORD);
            Assert.Equal(t.Topic, TOPIC);
            Assert.Equal(t.BusUsername, BUSUSERNAME);
            Assert.Equal(t.Password, PASSWORD);
        }

        [Fact]
        public void DefaultTopicTest()
        {
            var str = "teste";
            AbstractBusConfigurations.DefaultTopic = str;
            Assert.Equal(AbstractBusConfigurations.DefaultTopic, str);
        }

        [Fact]
        public void IsEnabledTest()
        {
            IConfiguration _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { })
                .Build();
            var t = new TestBusConfigurationsClass(_configuration);
            Assert.True(t.IsEnabled());
        }

        public class TestBusConfigurationsClass : AbstractBusConfigurations
        {
            public TestBusConfigurationsClass(IConfiguration configuration) : base(configuration) { }

            public override bool IsEnabled() => true;
        }
    }
}
