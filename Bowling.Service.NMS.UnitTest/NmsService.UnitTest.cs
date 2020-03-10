using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bowling.Service.NMS.UnitTest
{
    [TestClass]
    public class NmsServiceUnitTest
    {

        [TestMethod]
        public void TestConnectionStartAndGetConnectionStatus()
        {
            /*
                        var s = new NmsService();
                        var c = new NmsConfigurations();
                        s.ConnectionStart(c);
                        Assert.AreEqual<ConnectionStatus>(ConnectionStatus.NOT_CONFIGURED, s.GetConnectionStatus(), "Test Conficuration for NOT_CONFIGURED");
                        c.Uri = new Uri("activemq:tcp://test-none:0?wireFormat.tightEncodingEnabled=true");
                        s.ConnectionStart(c);
                        Assert.AreEqual<ConnectionStatus>(ConnectionStatus.ERROR, s.GetConnectionStatus(), "Test Conficuration for ERROR");

                        c.Uri = new Uri("activemq:tcp://localhost:61616?wireFormat.tightEncodingEnabled=true");
                        c.Username = "user-amq";
                        c.Password = "password-amq";
                        c.Topic = "Bowling.Service.NMS.UnitTest";
                        s.ConnectionStart(c);
                        Assert.AreEqual<ConnectionStatus>(ConnectionStatus.CONNECTED, s.GetConnectionStatus(), "Test Conficuration for CONNECTED");
            */
        }
    }
}
