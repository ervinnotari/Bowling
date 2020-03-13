using Bowling.Domain.Game.Interfaces;
using M2Mqtt;
using M2Mqtt.Messages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bowling.Service.Bus.MQTT
{
    public class MqttService : IBusService, IDisposable
    {
        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<IBusService.ConnectionStatus> OnStatusChange;
        private MqttClient _client;
        private Exception _error;
        private readonly MqttConfiguration _configuration;
        private List<Action> _actions = new List<Action>();

        public MqttService(IConfiguration configuration)
        {
            _configuration = new MqttConfiguration(configuration);
        }

        public IBusService.ConnectionStatus GetConnectionStatus()
        {
            if (_error != null) return IBusService.ConnectionStatus.ERROR;
            else if (_client == null) return IBusService.ConnectionStatus.DISABLED;
            else if (!_client.IsConnected) return IBusService.ConnectionStatus.CONECTING;
            else if (_client.IsConnected) return IBusService.ConnectionStatus.CONNECTED;
            else return IBusService.ConnectionStatus.DISABLED;
        }

        public void OnObjectReciver<T>(Action<T> listener)
        {
            _client.MqttMsgPublishReceived += (object sender, MqttMsgPublishEventArgs e) =>
            {
                try
                {
                    var txtMsg = Encoding.UTF8.GetString(e.Message);
                    var stt = new Newtonsoft.Json.JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error };
                    var obj = JsonConvert.DeserializeObject<T>(txtMsg, stt);
                    listener.Invoke(obj);
                }
                catch (Exception) { }
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
                    _client = new MqttClient(_configuration.Host) { ProtocolVersion = MqttProtocolVersion.Version_3_1 };
                    var code = _client.Connect(Guid.NewGuid().ToString());
                    _client.Subscribe(new string[] { _configuration.Topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                    _client.MqttMsgPublishReceived += (object sender, MqttMsgPublishEventArgs e) => OnMessageReciver?.Invoke(e);
                    OnConnection?.Invoke(code);
                }
                catch (Exception e)
                {
                    _error = e;
                }
                OnStatusChange?.Invoke(GetConnectionStatus());
            });
        }

        public Exception GetError() => _error;

        public void Dispose()
        {
            if (_client != null && _client.IsConnected)
            {
                _client.Disconnect();
            }
        }
    }
}