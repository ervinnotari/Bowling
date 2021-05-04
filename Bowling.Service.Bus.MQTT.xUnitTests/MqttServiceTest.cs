using Microsoft.Extensions.Configuration;
using System;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bowling.Domain.Game.Interfaces;

namespace Bowling.Service.Bus.MQTT.xUnitTests
{
    public class MqttServiceTest
    {
        private readonly IConfiguration _configuration;

        public MqttServiceTest()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"BrokerUri", "tcp://user:pass@broker.mqttdashboard.com:1883/topic/bowling/MQTT_xUnitTests"}
                }).Build();
        }

        [Fact]
        public void GetConnectionStatusTest()
        {
            Task.Run(async () =>
            {

                IBusService.ConnectionStatus value;
                using var mqtt = new MqttService();
                var builder = new Uri("tcp://user:pass@broker.mqttdashboard.com:1883/topic/bowling/MQTT_xUnitTests");

                await mqtt.ConnectionStopAsync();
                mqtt.OnMessageReciver += Mqtt_OnMessageReciver;
                mqtt.OnConnection += Mqtt_OnConnection;
                mqtt.OnStatusChange += (IBusService.ConnectionStatus arg1, IBusService.ConnectionInfo arg2) =>
                {
                    Assert.Equal(arg2.BrokerUri, builder);
                };

                value = mqtt.GetConnectionStatus();
                Assert.Equal(IBusService.ConnectionStatus.Disabled, value);

                await mqtt.ConnectionStartAsync(builder);
                value = mqtt.GetConnectionStatus();
                Assert.Equal(IBusService.ConnectionStatus.Connected, value);
                Assert.Null(mqtt.GetError());
                InternalConfigurationTest(mqtt);

                await mqtt.ConnectionStopAsync();
                await Task.Delay(1000);
                value = mqtt.GetConnectionStatus();
                Assert.Equal(IBusService.ConnectionStatus.Disabled, value);

                try
                {
                    builder = new Uri("tcp://user:pass@0.0.0.0:1883/topic/bowling/MQTT_xUnitTests");
                    value = mqtt.GetConnectionStatus();
                    Assert.Equal(IBusService.ConnectionStatus.Disabled, value);

                    await mqtt.ConnectionStartAsync(builder);
                    value = mqtt.GetConnectionStatus();
                    Assert.Equal(IBusService.ConnectionStatus.Error, value);
                    Assert.NotNull(mqtt.GetError());
                }
                finally
                {
                    //ignore
                }
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void SendAndReciverMensageObjectNotConvert()
        {
            Task.Run(async () =>
            {
                var test1 = new KeyValuePair<int, int>(0, 1);
                var test2 = new Version(1, 0, 0);
                var result = default(Version);
                using var mqtt = new MqttService(_configuration);
                await mqtt.ConnectionStartAsync();
                var value = mqtt.GetConnectionStatus();
                Assert.Equal(IBusService.ConnectionStatus.Connected, value);
                mqtt.OnObjectReciver<Version>((o) => { Assert.Equal(test2, o); });
                mqtt.SendObject(test1);
                await Task.Delay(2000);
                Assert.Null(result);
                mqtt.SendObject(test2);
                await Task.Delay(2000);
            }).GetAwaiter().GetResult();
        }

        [Theory]
        [InlineData("tcp://user:pass@broker.mqttdashboard.com:1883/topic/bowling/MQTT_xUnitTests")]
        [InlineData("tcp://broker.mqttdashboard.com:1883/topic/bowling/MQTT_xUnitTests")]
        [InlineData("tcp://broker.mqttdashboard.com:1883/bowling/MQTT_xUnitTests")]
        [InlineData("tcp://broker.mqttdashboard.com:1883")]
        public void ConfigurationTest(string login)
        {
            var builder = new UriBuilder(login);
            var mqtt = new MqttService();
            mqtt.ConnectionStart(builder.Uri);
            mqtt.OnMessageReciver += Mqtt_OnMessageReciver;
            InternalConfigurationTest(mqtt);
            Assert.NotNull(mqtt);
        }

        [Fact]
        public void ConfigurationTest2()
        {
            var mqtt = new MqttService(_configuration);
            mqtt.ConnectionStart();
            InternalConfigurationTest(mqtt);
            mqtt.ConnectionStop();
            Assert.NotNull(mqtt);
        }

        private void InternalConfigurationTest(MqttService mqtt)
        {
            mqtt.SendObject(156.5);
            mqtt.SendText("test");
        }

        private void Mqtt_OnMessageReciver(object obj) => Assert.NotNull(obj);

        private void Mqtt_OnConnection(object obj) => Assert.NotNull(obj);
    }
}
