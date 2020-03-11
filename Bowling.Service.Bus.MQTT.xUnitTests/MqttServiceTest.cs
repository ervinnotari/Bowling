using System;
using Xunit;

namespace Bowling.Service.Bus.MQTT.xUnitTests
{
    public class MqttServiceTest
    {
        [Fact]
        public void Test1()
        {
            var t = new MqttService(new MqttConfiguration(new Uri("tcp://localhost:1883"), null, null, "bowling/play"));
            t.ConnectionStart();
            t.SendObject(null);
            Assert.True(true, "oi");
        }
    }
}