using Bowling.Domain.Game.Interfaces;
using System;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using M2Mqtt;
using M2Mqtt.Messages;
using Microsoft.Extensions.Configuration;

namespace Bowling.Service.Bus.MQTT
{
    public class MqttService : IBusService, IDisposable
    {
        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<IBusService.ConnectionStatus> OnStatusChange;
        private MqttClient _client;
        private Exception _error;
        private IConfiguration _configuration;

        public MqttService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IBusService.ConnectionStatus GetConnectionStatus()
        {
            return IBusService.ConnectionStatus.ERROR;
        }

        public void OnObjectReciver<T>(Action<T> listener)
        {
        }

        public void SendText(string message)
        {
            var payload = Encoding.UTF8.GetBytes(message);
            const byte qos = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE;
            var msgId = _client.Publish(MqttConfiguration.Topic, payload, qos, true);
        }

        public void SendObject(object obj)
        {
        }

        public void ConnectionStart()
        {
            ConnectionStartAsync().Wait();
        }

        public async Task ConnectionStartAsync()
        {
            try
            {
                _client = new MqttClient(MqttConfiguration.Host) {ProtocolVersion = MqttProtocolVersion.Version_3_1};
                var code = _client.Connect(Guid.NewGuid().ToString());
            }
            catch (Exception e)
            {
                _error = e;
            }
        }

        public Exception GetError() => _error;

        public void Dispose()
        {
            if (_client.IsConnected)
                _client.Disconnect();
        }
    }
}