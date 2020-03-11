using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Bowling.Domain.Game.Interfaces;
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
        public NmsConfiguration Configuration { get; set; }

        public NmsService(NmsConfiguration configuration)
        {
            Configuration = configuration;
        }

        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<IBusService.ConnectionStatus> OnStatusChange;

        public IBusService.ConnectionStatus GetConnectionStatus()
        {
            if (Error != null) return IBusService.ConnectionStatus.ERROR;
            else if (Connection == null) return IBusService.ConnectionStatus.DISABLED;
            else if (!Connection.IsStarted && Session == null) return IBusService.ConnectionStatus.CONECTING;
            else if (Connection.IsStarted && Session != null) return IBusService.ConnectionStatus.CONNECTED;
            else return IBusService.ConnectionStatus.DISABLED;
        }
        public void OnObjectReciver<T>(Action<T> listener)
        {
            IMessageConsumer consumer = Session.CreateConsumer(Destination);
            consumer.Listener += msg =>
            {
                if (msg is ITextMessage txtMsg)
                {
                    try
                    {
                        var stt = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error };
                        var obj = JsonConvert.DeserializeObject<T>(txtMsg.Text, stt);
                        listener.Invoke(obj);
                    }
                    catch (JsonSerializationException) { }
                }
            };
        }
        public void SendText(string message)
        {
            IMessageProducer producer = Session.CreateProducer(Destination);
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

            if (Configuration.Uri != null)
            {
                try
                {
                    var factory = new ConnectionFactory(Configuration.Uri);
                    if (Configuration.Username != null && Configuration.Password != null)
                        Connection = factory.CreateConnection(Configuration.Username, Configuration.Password);
                    else
                        Connection = factory.CreateConnection();

                    Connection.Start();
                    Session = Connection.CreateSession();
                    Destination = Session.GetTopic(Configuration.Topic);
                    IMessageConsumer consumer = Session.CreateConsumer(Destination);
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
    }
}
