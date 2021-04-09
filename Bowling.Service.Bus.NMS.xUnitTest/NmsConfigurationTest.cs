using System;
using System.Collections.Generic;
using Moq;
using Bowling.Infra.Utilities;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Bowling.Service.Bus.NMS.xUnitTest
{
    public class NmsConfigurationTest
    {
        [Fact]
        public void IsEnableTrueTeste()
        {
            var conf = new Mock<IConfiguration>();
            conf.SetupGet(c => c["Url"]).Returns("tcp://localhost:61616");
            conf.SetupGet(c => c["Topic"]).Returns("bowling/NMS_xUnitTests");
            conf.SetupGet(c => c["BusUsername"]).Returns("teste_user");
            conf.SetupGet(c => c["Password"]).Returns("teste_pass");
            var nmsc = new NmsConfiguration(conf.Object);
            var value = nmsc.IsEnabled();
            Assert.True(value);
            Assert.Equal(new Uri("tcp://localhost:61616"), nmsc.Uri);
            Assert.Equal("bowling/NMS_xUnitTests", nmsc.Topic);
            Assert.Equal("teste_user", nmsc.BusUsername);
            Assert.Equal("teste_pass", nmsc.Password);
        }
        [Fact]
        public void IsEnableFalseTeste()
        {
            AbstractBusConfigurations.DefaultTopic = "bowling/NMS_xUnitTests";
            var conf = new Mock<IConfiguration>();
            var nmsc = new NmsConfiguration(conf.Object);
            var value = nmsc.IsEnabled();
            Assert.False(value);
            Assert.Equal("bowling/NMS_xUnitTests", nmsc.Topic);
            Assert.Null(nmsc.Password);
            Assert.Null(nmsc.BusUsername);
            Assert.Null(nmsc.Uri);
        }
    }
}
