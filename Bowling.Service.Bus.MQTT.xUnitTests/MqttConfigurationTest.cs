using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Xunit;
using System.Collections.Generic;


namespace Bowling.Service.Bus.MQTT.xUnitTests
{
    public class MqttConfigurationTest
    {
        [Fact]
        public void IsEnableTrueTeste()
        {
            var conf = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Host", "broker.mqttdashboard.com" },
                    { "Topic", "bowling/MQTT_xUnitTests" },
                    { "Username", "teste" },
                    { "Pasword", "teste" },
                    { "Port", "1883" }
                }).Build();
            var mqttc = new MqttConfiguration(conf);
            var value = mqttc.IsEnabled();
            Assert.True(value);
        }
        [Fact]
        public void IsEnableFalseTeste()
        {
            var conf = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddInMemoryCollection().Build();
            var mqttc = new MqttConfiguration(conf);
            var value = mqttc.IsEnabled();
            Assert.False(value);
        }
    }
}