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
                //.SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    {"Host", "broker.mqttdashboard.com"},
                    {"Topic", "bowling/MQTT_xUnitTests"},
                    {"Port", "1883"}
                }).Build();
        }

        [Fact]
        public async void GetConnectionStatusTest()
        {
            IBusService.ConnectionStatus value;
            var mqtt = new MqttService(_configuration);
            value = mqtt.GetConnectionStatus();
            Assert.Equal(IBusService.ConnectionStatus.Disabled, value);

            await mqtt.ConnectionStartAsync();
            value = mqtt.GetConnectionStatus();
            Assert.Equal(IBusService.ConnectionStatus.Connected, value);
            Assert.Null(mqtt.GetError());

            var bkp = _configuration["Host"];
            try
            {
                _configuration["Host"] = "****.***";
                mqtt = new MqttService(_configuration);
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
        }

        [Fact]
        public async void SendAndReciverMensageTest()
        {
            var test = $"{(new Random()).Next(15292, 55292)}";
            var test2 = new Version(1, 0, 0);
            var result = default(Version);

            var mqtt = new MqttService(_configuration);
            await mqtt.ConnectionStartAsync();
            mqtt.OnObjectReciver<Version>((o) => { result = o; });
            mqtt.SendText(test);
            mqtt.SendObject(test2);

            await Task.Delay(2000);
            Assert.Equal(test2, result);
        }

        [Fact]
        public void SendObjectTest()
        {
            var mqtt = new MqttService(_configuration);
            mqtt.ConnectionStart();
            mqtt.SendObject(156.5);
            mqtt.SendText("test");
        }
    }
}