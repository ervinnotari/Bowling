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
        private readonly IConfiguration _configuration2;

        public MqttServiceTest()
        {
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Host", "broker.mqttdashboard.com"},
                    {"Topic", "bowling/MQTT_xUnitTests"},
                    {"Port", "1883"}
                }).Build();
            _configuration2 = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Host", "broker.mqttdashboard.com"},
                    {"Topic", "bowling/MQTT_xUnitTests"},
                    {"BusUsername", "teste"},
                    {"Password", "teste"}
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

                    mqtt.OnMessageReciver += Mqtt_OnMessageReciver;
                    mqtt.OnConnection += Mqtt_OnConnection;
                    mqtt.OnStatusChange += Mqtt_OnStatusChange;
                    value = mqtt.GetConnectionStatus();
                    Assert.Equal(IBusService.ConnectionStatus.Disabled, value);

                    await mqtt.ConnectionStartAsync();
                    value = mqtt.GetConnectionStatus();
                    Assert.Equal(IBusService.ConnectionStatus.Connected, value);
                    Assert.Null(mqtt.GetError());
                    GC.SuppressFinalize(mqtt);
                    mqtt.Dispose();
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
        public void SendAndReciverMensageTest()
        {
            Task.Run(async () =>
            {
                var test = $"{(new Random()).Next(15292, 55292)}";
                var test2 = new Version(1, 0, 0);
                var result = default(Version);

                using var mqtt = new MqttService(_configuration);
                await mqtt.ConnectionStartAsync();
                mqtt.OnObjectReciver<Version>((o) => { result = o; });
                mqtt.SendText(test);
                mqtt.SendObject(test2);

                await Task.Delay(2000);
                Assert.Equal(test2, result);
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void ConfigurationWichUserTest() => ConfigurationTest(_configuration);
        [Fact]
        public void ConfigurationNoUserTest() => ConfigurationTest(_configuration2);

        private void ConfigurationTest(IConfiguration conf)
        {
            var mqtt = new MqttService(conf);
            mqtt.ConnectionStart();
            mqtt.OnMessageReciver += Mqtt_OnMessageReciver;
            mqtt.SendObject(156.5);
            mqtt.SendText("test");
        }

        private void Mqtt_OnStatusChange(IBusService.ConnectionStatus arg1, object arg2) => Assert.NotNull(arg2);

        private void Mqtt_OnMessageReciver(object obj) => Assert.NotNull(obj);

        private void Mqtt_OnConnection(object obj) => Assert.NotNull(obj);
    }
}