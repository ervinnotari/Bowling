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
    public sealed class MqttService : IBusService
    {
        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<IBusService.ConnectionStatus, IBusService.ConnectionInfo> OnStatusChange;

        private IBusService.ConnectionInfo _info;
        private MqttClient _client;
        private Exception _error;
        private readonly MqttConfiguration _configuration;

        public MqttService() { }

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
            _client.Publish(_info.Topic, payload, qos, false);
        }

        public void SendObject(object obj)
        {
            var strObj = JsonConvert.SerializeObject(obj);
            SendText(strObj);
        }

        public void ConnectionStart()
        {
            ConnectionStart(_configuration.BrokerUri);
        }

        public void ConnectionStart(Uri uri)
        {
            ConnectionStartAsync(uri).Wait();
        }

        public async Task ConnectionStartAsync()
        {
            await ConnectionStartAsync(_configuration.BrokerUri);
        }

        public async Task ConnectionStartAsync(Uri uri)
        {
            await Task.Run(() =>
            {
                try
                {
                    _info = new IBusService.ConnectionInfo(uri, MqttConfiguration.TopicMatcher(uri));
                    _error = null;
                    _client = new MqttClient(uri.Host, uri.Port, false, null, null, MqttSslProtocols.None) { ProtocolVersion = MqttProtocolVersion.Version_3_1 };
                    var code = default(byte);
                    if (string.IsNullOrEmpty(uri.UserInfo))
                    {
                        code = _client.Connect(Guid.NewGuid().ToString());
                    }
                    else
                    {
                        var bUri = new UriBuilder(uri);
                        code = _client.Connect(Guid.NewGuid().ToString(), bUri.UserName, bUri.Password);
                    }
                    _client.Subscribe(new[] { _info.Topic }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                    _client.MqttMsgPublishReceived += DefaultClient_MqttMsgPublishReceived;
                    OnConnection?.Invoke(code);
                }
                catch (Exception e)
                {
                    _error = e;
                }
                finally
                {
                    OnStatusChange?.Invoke(GetConnectionStatus(), _info);
                }
            });
        }

        private void DefaultClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            OnMessageReciver?.Invoke(e);
        }

        public Exception GetError() => _error;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
                ConnectionStop();
        }

        public void ConnectionStop()
        {
            ConnectionStopAsync().Wait();
        }

        public async Task ConnectionStopAsync()
        {
            await Task.Run(() =>
            {
                if (_client != null && _client.IsConnected)
                {
                    _client.Disconnect();
                    OnStatusChange?.Invoke(GetConnectionStatus(), _info);
                }
            });
        }
    }
}
