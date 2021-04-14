using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using Bowling.Domain.Game.Interfaces;
using Bowling.Domain.Game.Exceptions;
using Apache.NMS;
using System.Threading.Tasks;

namespace Bowling.Service.Bus.NMS.xUnitTest
{
    public class NmsServiceTest
    {
        [Fact]
        public void UrlNotInformateTest()
        {
            var _conf = new Mock<IConfiguration>();
            _conf.SetupGet(c => c["Url"]).Returns("");
            _conf.SetupGet(c => c["Topic"]).Returns("");
            _conf.SetupGet(c => c["BusUsername"]).Returns("");
            _conf.SetupGet(c => c["Password"]).Returns("");

            using var srv = new NmsService(_conf.Object);
            Assert.Equal<IBusService.ConnectionStatus>(IBusService.ConnectionStatus.Disabled, srv.GetConnectionStatus());
            srv.ConnectionStart();
            Assert.Equal<IBusService.ConnectionStatus>(IBusService.ConnectionStatus.Error, srv.GetConnectionStatus());
            Assert.IsType<InvalidArgumentExecption>(srv.GetError());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ConnectionTest(bool userCredentials)
        {
            var _conf = new Mock<IConfiguration>();
            _conf.SetupGet(c => c["Url"]).Returns("mock://teste");
            _conf.SetupGet(c => c["Topic"]).Returns("teste");
            if (userCredentials)
            {
                _conf.SetupGet(c => c["BusUsername"]).Returns("admin");
                _conf.SetupGet(c => c["Password"]).Returns("123");
            }
            using var srv = new NmsService(_conf.Object);
            srv.OnStatusChange += (st, ob) =>
            {
                Assert.IsType<IBusService.ConnectionStatus>(st);
                Assert.NotNull(ob);
            };
            Assert.Equal<IBusService.ConnectionStatus>(IBusService.ConnectionStatus.Disabled, srv.GetConnectionStatus());
            srv.ConnectionStart();
            Assert.Equal<IBusService.ConnectionStatus>(IBusService.ConnectionStatus.Connected, srv.GetConnectionStatus());
        }

        [Fact]
        public void SendAndReciverMensageTest()
        {
            Task.Run(async () =>
            {
                var confMock = new Mock<IConfiguration>();
                confMock.SetupGet(c => c["Url"]).Returns("mock://teste");
                confMock.SetupGet(c => c["Topic"]).Returns("teste");

                var srv = new NmsService(confMock.Object);
                srv.OnMessageReciver += (ob) => {};
                await srv.ConnectionStartAsync();
                srv.SendText("hello");
            }).GetAwaiter().GetResult();
        }

        [Theory]
        [InlineData(1)]
        [InlineData("teste")]
        [InlineData('o')]
        [InlineData(1.56)]
        public void SendObjectTest(object obj)
        {
            Task.Run(async () =>
            {
                static void teste(object ob) { }
                var confMock = new Mock<IConfiguration>();
                confMock.SetupGet(c => c["Url"]).Returns("mock://teste");
                confMock.SetupGet(c => c["Topic"]).Returns("teste");

                var srv = new NmsService(confMock.Object);
                await srv.ConnectionStartAsync();
                srv.OnObjectReciver<object>(teste);
                srv.SendObject(obj);
            }).GetAwaiter().GetResult();
        }
    }
}