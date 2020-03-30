using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Bowling.Domain.Game.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Bowling.Service.Bus.NMS
{
    public sealed class NmsService : IBusService
    {
        public Exception Error { get; private set; }
        private IConnection _connection;
        private ISession _session;
        private IDestination _destination;

        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<IBusService.ConnectionStatus, object> OnStatusChange;
        private readonly NmsConfiguration _configuration;
        private bool _disposed;

        public NmsService(IConfiguration configuration)
        {
            _configuration = new NmsConfiguration(configuration);
        }

        public IBusService.ConnectionStatus GetConnectionStatus()
        {
            if (Error != null) return IBusService.ConnectionStatus.Error;
            else if (_connection == null) return IBusService.ConnectionStatus.Disabled;
            else if (!_connection.IsStarted && _session == null) return IBusService.ConnectionStatus.Conecting;
            else if (_connection.IsStarted && _session != null) return IBusService.ConnectionStatus.Connected;
            else return IBusService.ConnectionStatus.Disabled;
        }

        public void OnObjectReciver<T>(Action<T> listener)
        {
            var consumer = _session.CreateConsumer(_destination);
            consumer.Listener += msg =>
            {
                if (!(msg is ITextMessage txtMsg)) return;
                try
                {
                    var stt = new JsonSerializerSettings() {MissingMemberHandling = MissingMemberHandling.Error};
                    var obj = JsonConvert.DeserializeObject<T>(txtMsg.Text, stt);
                    listener.Invoke(obj);
                }
                catch (JsonSerializationException)
                {
                    // ignored
                }
            };
        }

        public void SendText(string message)
        {
            var producer = _session.CreateProducer(_destination);
            var msg = producer.CreateTextMessage(message);
            producer.Send(msg);
        }

        public void SendObject(object obj)
        {
            var strObj = JsonConvert.SerializeObject(obj);
            SendText(strObj);
        }

        public void ConnectionStart()
        {
            if (_configuration.Uri != null)
            {
                try
                {
                    var factory = new ConnectionFactory(_configuration.Uri);
                    if (_configuration.BusUsername != null && _configuration.Password != null)
                        _connection = factory.CreateConnection(_configuration.BusUsername, _configuration.Password);
                    else
                        _connection = factory.CreateConnection();

                    _connection.Start();
                    _session = _connection.CreateSession();
                    _destination = _session.GetTopic(_configuration.Topic);
                    var consumer = _session.CreateConsumer(_destination);
                    consumer.Listener += (IMessage message) => OnMessageReciver?.Invoke(message);
                    OnConnection?.Invoke(_connection);
                }
                catch (Exception ex)
                {
                    Error = ex;
                }
            }

            OnStatusChange?.Invoke(GetConnectionStatus(), _configuration);
        }

        public Task ConnectionStartAsync() => Task.Factory.StartNew(ConnectionStart);
        public Exception GetError() => Error;
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _connection?.Dispose();
            }

            _disposed = true;
        }
    }
}