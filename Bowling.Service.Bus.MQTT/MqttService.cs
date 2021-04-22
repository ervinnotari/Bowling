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
    public sealed class MqttService : IBusService
    {
        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<IBusService.ConnectionStatus, object> OnStatusChange;
        private MqttClient _client;
        private Exception _error;
        private readonly MqttConfiguration _configuration;

        public MqttService(string topic)
        {
            var iconfg = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string> { { "Topic", topic } }).Build();
            _configuration = new MqttConfiguration(iconfg);
        }

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
            ConnectionStart(_configuration.Host, _configuration.Port, _configuration.BusUsername, _configuration.Password);
        }

        public void ConnectionStart(string host, int port)
        {
            ConnectionStartAsync(host, port).Wait();
        }

        public void ConnectionStart(string host, int port, string username, string password)
        {
            ConnectionStartAsync(host, port, username, password).Wait();
        }

        public async Task ConnectionStartAsync()
        {
            await ConnectionStartAsync(_configuration.Host, _configuration.Port, _configuration.BusUsername, _configuration.Password);
        }

        public async Task ConnectionStartAsync(string host, int port)
        {
            await ConnectionStartAsync(host, port, null, null);
        }

        public async Task ConnectionStartAsync(string host, int port, string username, string password)
        {
            await Task.Run(() =>
            {
                try
                {
                    _error = null;
                    _client = new MqttClient(host, port, false, null, null, MqttSslProtocols.None) { ProtocolVersion = MqttProtocolVersion.Version_3_1 };
                    var code = default(byte);
                    if (string.IsNullOrEmpty(username + password))
                    {
                        code = _client.Connect(Guid.NewGuid().ToString());
                    }
                    else
                    {
                        code = _client.Connect(Guid.NewGuid().ToString(), username, password);
                    }
                    _client.Subscribe(new[] { _configuration.Topic }, new[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                    _client.MqttMsgPublishReceived += DefaultClient_MqttMsgPublishReceived;
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
                    OnStatusChange?.Invoke(GetConnectionStatus(), _configuration);
                }
            });
        }
    }
}
