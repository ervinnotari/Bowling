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
        private IConfiguration _configuration;

        public MqttServiceTest()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "Host", "broker.mqttdashboard.com" },
                    { "Topic", "bowling/MQTT_xUnitTests" },
                    { "Port", "1883" }
                }).Build();
        }

        [Fact]
        public async void GetConnectionStatusTest()
        {
            IBusService.ConnectionStatus value;
            using (var _mqtt = new MqttService(_configuration))
            {
                value = _mqtt.GetConnectionStatus();
                Assert.Equal(IBusService.ConnectionStatus.DISABLED, value);
                value = default;

                await _mqtt.ConnectionStartAsync();
                value = _mqtt.GetConnectionStatus();
                Assert.Equal(IBusService.ConnectionStatus.CONNECTED, value);
                value = default;
            }
            var bkp = _configuration["Host"];
            try
            {
                _configuration["Host"] = "****.***";
                using var _mqtt = new MqttService(_configuration);
                value = _mqtt.GetConnectionStatus();
                Assert.Equal(IBusService.ConnectionStatus.DISABLED, value);
                value = default;

                await _mqtt.ConnectionStartAsync();
                value = _mqtt.GetConnectionStatus();
                Assert.Equal(IBusService.ConnectionStatus.ERROR, value);
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

            using var _mqtt = new MqttService(_configuration);
            await _mqtt.ConnectionStartAsync();
            _mqtt.OnObjectReciver<string>((o) => result = o);
            _mqtt.SendText(test);

            await Task.Delay(1000);
            Assert.Equal(test, result);
        }


        [Fact]
        public async void SendMensageTest()
        {
            var test = $"{(new Random()).Next(15292, 55292)}";
            var result = default(string);
            using var _mqtt = new MqttService(_configuration);
            _mqtt.ConnectionStart();
            _mqtt.OnObjectReciver<string>((o) => result = o);

            _mqtt.SendText(test);
            await Task.Delay(1000);
            Assert.Equal(test, result);

            test = $"{(new Random()).Next(15292, 55292)}";
            _mqtt.SendObject(test);
            await Task.Delay(1000);
            Assert.Equal(test, result);
        }


    }
}