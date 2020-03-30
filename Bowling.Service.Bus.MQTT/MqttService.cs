using Bowling.Domain.Game.Interfaces;
using M2Mqtt;
using M2Mqtt.Messages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Service.Bus.MQTT
{
    public class MqttService : IBusService
    {
        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<IBusService.ConnectionStatus, object> OnStatusChange;
        private MqttClient _client;
        private Exception _error;
        private readonly MqttConfiguration _configuration;

        public MqttService(IConfiguration configuration)
        {
            _configuration = new MqttConfiguration(configuration);
        }

        public IBusService.ConnectionStatus GetConnectionStatus()
        {
            if (_error != null)
                return IBusService.ConnectionStatus.Error;
            else if (_client != null && _client.IsConnected)
                return IBusService.ConnectionStatus.Connected;
            return IBusService.ConnectionStatus.Disabled;
        }

        public void OnObjectReciver<T>(Action<T> listener)
        {
            _client.MqttMsgPublishReceived += (sender, e) =>
            {
                try
                {
                    var txtMsg = Encoding.UTF8.GetString(e.Message);
                    var stt = new JsonSerializerSettings()
                    { MissingMemberHandling = MissingMemberHandling.Error };
                    var obj = JsonConvert.DeserializeObject<T>(txtMsg, stt);
                    listener.Invoke(obj);
                }
                catch (JsonException)
                {
                    // ignored
                }
            };
        }

        public void SendText(string message)
        {
            var payload = Encoding.UTF8.GetBytes(message);
            const byte qos = MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE;
            _client.Publish(_configuration.Topic, payload, qos, false);
        }

        public void SendObject(object obj)
        {
            var strObj = JsonConvert.SerializeObject(obj);
            SendText(strObj);
        }

        public void ConnectionStart()
        {
            ConnectionStartAsync().Wait();
        }

        public async Task ConnectionStartAsync()
        {
            await Task.Run(() =>
            {
                try
                {
                    _error = null;
                    _client = new MqttClient(_configuration.Host, _configuration.Port, false, null,
                            null, MqttSslProtocols.None)
                    { ProtocolVersion = MqttProtocolVersion.Version_3_1 };
                    var auth = !string.IsNullOrEmpty(_configuration.BusUsername + _configuration.Password);
                    var code = auth
                        ? _client.Connect(Guid.NewGuid().ToString())
                        : _client.Connect(Guid.NewGuid().ToString(), _configuration.BusUsername, _configuration.Password);
                    _client.Subscribe(new[] { _configuration.Topic },
                        new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                    _client.MqttMsgPublishReceived += (sender, e) =>
                        OnMessageReciver?.Invoke(e);
                    OnConnection?.Invoke(code);
                }
                catch (Exception e)
                {
                    _error = e;
                }
                finally
                {
                    OnStatusChange?.Invoke(GetConnectionStatus(), _configuration);
                }
            });
        }

        public Exception GetError() => _error;

        ~MqttService()
        {
            this.Dispose();
        }

        public void Dispose()
        {
            if (_client != null && _client.IsConnected)
            {
                _client.Disconnect();
            }
        }
    }
}