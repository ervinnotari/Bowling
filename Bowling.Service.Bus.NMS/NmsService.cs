using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Bowling.Domain.Game.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Bowling.Service.Bus.NMS
{
    public class NmsService : IBusService
    {
        public Exception Error { get; protected set; }
        protected IConnection Connection { get; private set; }
        protected ISession Session { get; private set; }
        protected IDestination Destination { get; private set; }

        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<IBusService.ConnectionStatus> OnStatusChange;
        private NmsConfiguration _configuration;

        public NmsService(IConfiguration configuration)
        {
            _configuration = new NmsConfiguration(configuration);
        }

        public IBusService.ConnectionStatus GetConnectionStatus()
        {
            if (Error != null) return IBusService.ConnectionStatus.Error;
            else if (Connection == null) return IBusService.ConnectionStatus.Disabled;
            else if (!Connection.IsStarted && Session == null) return IBusService.ConnectionStatus.Conecting;
            else if (Connection.IsStarted && Session != null) return IBusService.ConnectionStatus.Connected;
            else return IBusService.ConnectionStatus.Disabled;
        }
        public void OnObjectReciver<T>(Action<T> listener)
        {
            var consumer = Session.CreateConsumer(Destination);
            consumer.Listener += msg =>
            {
                if (!(msg is ITextMessage txtMsg)) return;
                try
                {
                    var stt = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error };
                    var obj = JsonConvert.DeserializeObject<T>(txtMsg.Text, stt);
                    listener.Invoke(obj);
                }
                catch (JsonSerializationException) { }
            };
        }
        public void SendText(string message)
        {
            var producer = Session.CreateProducer(Destination);
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
                    if (_configuration.Username != null && _configuration.Password != null)
                        Connection = factory.CreateConnection(_configuration.Username, _configuration.Password);
                    else
                        Connection = factory.CreateConnection();

                    Connection.Start();
                    Session = Connection.CreateSession();
                    Destination = Session.GetTopic(_configuration.Topic);
                    var consumer = Session.CreateConsumer(Destination);
                    consumer.Listener += (IMessage message) => OnMessageReciver?.Invoke(message);
                    OnConnection?.Invoke(Connection);
                }
                catch (Exception ex)
                {
                    Error = ex;
                }
            }
            OnStatusChange?.Invoke(GetConnectionStatus());
        }
        public Task ConnectionStartAsync() => Task.Factory.StartNew(ConnectionStart);
        public Exception GetError() => Error;
        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
