using Microsoft.Extensions.Configuration;
using System;
using System.IO;
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
            using (var mqtt = new MqttService(_configuration))
            {
                value = mqtt.GetConnectionStatus();
                Assert.Equal(IBusService.ConnectionStatus.Disabled, value);
                value = default;

                await mqtt.ConnectionStartAsync();
                value = mqtt.GetConnectionStatus();
                Assert.Equal(IBusService.ConnectionStatus.Connected, value);
                Assert.Null(mqtt.GetError());
                value = default;
            }

            var bkp = _configuration["Host"];
            try
            {
                _configuration["Host"] = "****.***";
                using var mqtt = new MqttService(_configuration);
                value = mqtt.GetConnectionStatus();
                Assert.Equal(IBusService.ConnectionStatus.Disabled, value);
                value = default;

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
            var result = default(string);

            using var mqtt = new MqttService(_configuration);
            await mqtt.ConnectionStartAsync();
            mqtt.OnObjectReciver<string>((o) => { result = o; });
            mqtt.OnObjectReciver<int>((o) => { });
            mqtt.SendText(test);
            mqtt.SendObject(new Version());

            await Task.Delay(1000);
            Assert.Equal(test, result);
        }

        [Fact]
        public void SendObjectTest()
        {
            using var mqtt = new MqttService(_configuration);
            mqtt.ConnectionStart();
            mqtt.SendObject(156.5);
            mqtt.SendText("test");
        }
    }
}