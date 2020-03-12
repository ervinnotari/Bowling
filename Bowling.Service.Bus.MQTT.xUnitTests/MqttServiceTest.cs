using System;
using Xunit;

namespace Bowling.Service.Bus.MQTT.xUnitTests
{
    public class MqttServiceTest
    {
        [Fact]
        public void Test1()
        {
            var t = new MqttService();
            t.ConnectionStart();
            t.SendText("oi");
            Assert.True(true, "oi");
        }
    }
}