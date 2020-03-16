using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Bowling.Service.Bus.NMS.xUnitTest
{
    public class NmsConfigurationTest
    {
        [Fact]
        public void IsEnableTrueTeste()
        {
            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Url", "tcp://localhost:61616" },
                    { "Topic", "bowling/NMS_xUnitTests" },
                    { "Username", "teste_user" },
                    { "Password", "teste_pass" },
                }).Build();
            var nmsc = new NmsConfiguration(conf);
            var value = nmsc.IsEnabled();
            Assert.True(value);
            Assert.Equal(new Uri("tcp://localhost:61616"), nmsc.Uri);
            Assert.Equal("bowling/NMS_xUnitTests", nmsc.Topic);
            Assert.Equal("teste_user", nmsc.Username);
            Assert.Equal("teste_pass", nmsc.Password);
        }
        [Fact]
        public void IsEnableFalseTeste()
        {
            var conf = new ConfigurationBuilder()
                .AddInMemoryCollection().Build();
            var nmsc = new NmsConfiguration(conf);
            var value = nmsc.IsEnabled();
            Assert.False(value);
            Assert.Equal("bowling/play", nmsc.Topic);
            Assert.Null(nmsc.Password);
            Assert.Null(nmsc.Username);
            Assert.Null(nmsc.Uri);
        }
    }
}
