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
                    {"Host", "broker.mqttdashboard.com"},
                    {"Topic", "bowling/MQTT_xUnitTests"},
                }).Build();
        }

        [Fact]
        public void GetConnectionStatusTest()
        {
            Task.Run(async () =>
            {
                IBusService.ConnectionStatus value;
                using (var mqtt = new MqttService(_configuration))
                {
                    await mqtt.ConnectionStopAsync();
                    mqtt.OnConnection += Mqtt_OnConnection;
                    mqtt.OnStatusChange += Mqtt_OnStatusChange;
                    value = mqtt.GetConnectionStatus();
                    Assert.Equal(IBusService.ConnectionStatus.Disabled, value);

                    await mqtt.ConnectionStartAsync();
                    value = mqtt.GetConnectionStatus();
                    Assert.Equal(IBusService.ConnectionStatus.Connected, value);
                    Assert.Null(mqtt.GetError());
                    InternalConfigurationTest(mqtt);

                    await mqtt.ConnectionStopAsync();
                    await Task.Delay(1000);
                    value = mqtt.GetConnectionStatus();
                    Assert.Equal(IBusService.ConnectionStatus.Disabled, value);
                }

                var bkp = _configuration["Host"];
                try
                {
                    _configuration["Host"] = "****.***";
                    using var mqtt = new MqttService(_configuration);
                    mqtt.OnConnection += Mqtt_OnConnection;
                    mqtt.OnStatusChange += Mqtt_OnStatusChange;
                    value = mqtt.GetConnectionStatus();
                    Assert.Equal(IBusService.ConnectionStatus.Disabled, value);

                    await mqtt.ConnectionStartAsync();
                    value = mqtt.GetConnectionStatus();
                    Assert.Equal(IBusService.ConnectionStatus.Error, value);
                    Assert.NotNull(mqtt.GetError());
                }
                finally
                {
                    _configuration["Host"] = bkp;
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
                mqtt.OnMessageReciver += Mqtt_OnMessageReciver;
                await mqtt.ConnectionStartAsync();
                mqtt.OnObjectReciver<Version>((o) => { Assert.Equal(test2, o); });
                mqtt.SendObject(test1);
                await Task.Delay(2000);
                Assert.Null(result);
                mqtt.SendObject(test2);
                await Task.Delay(2000);
            }).GetAwaiter().GetResult();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        [InlineData(true, "", "")]
        [InlineData(true, "TESTE", "TESTE")]
        public void ConfigurationTest(bool login, string user = null, string pass = null)
        {
            int port = 1883;
            string host = "broker.mqttdashboard.com";
            var mqtt = new MqttService("bowling/MQTT_xUnitTests");
            mqtt.OnMessageReciver += Mqtt_OnMessageReciver;
            if (login) mqtt.ConnectionStart(host, port, user, pass);
            else mqtt.ConnectionStart(host, port);
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

        private void Mqtt_OnStatusChange(IBusService.ConnectionStatus arg1, object arg2) => Assert.NotNull(arg2);

        private void Mqtt_OnMessageReciver(object obj) => Assert.NotNull(obj);

        private void Mqtt_OnConnection(object obj) => Assert.NotNull(obj);
    }
}
