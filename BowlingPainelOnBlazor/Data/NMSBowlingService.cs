using BowlingGame.Entities;
using BowlingPainelOnBlazor.Utils;
using Apache.NMS;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace BowlingPainelOnBlazor.Data
{
    public delegate void MessageListener<T>(T obj);
    public delegate void ConnectionEventHandler(IConnection conn);
    public enum ConnectionStatus { NOT_CONFIGURED, CONNECTED, CONECTING, ERROR }
    public class NMSBowlingService : IBowlingService
    {
        public event MessageListener OnMessageReciver;
        public event ConnectionEventHandler OnConnectionSucess;
        public event VoidEventHandler OnStatusChange;

        public NMSConfigurations Configurations { get; private set; }
        public IDestination Destination { get; private set; }
        public ISession Session { get; private set; }
        public IConnection Connection { get; private set; }
        public Exception Error { get; private set; }

        public NMSBowlingService(IOptionsMonitor<NMSConfigurations> options)
        {
            Configurations = options.CurrentValue;
            Task.Run(ConnectionStart);
        }

        private void ConnectionStart()
        {
            if (Configurations.Uri != null)
            {
                try
                {
                    Connection = Configurations.GetConnection();
                    Connection.Start();
                    Session = Connection.CreateSession();
                    Destination = Session.GetTopic(Configurations.Topic);
                    IMessageConsumer consumer = Session.CreateConsumer(Destination);
                    consumer.Listener += Consumer_Listener;
                    OnConnectionSucess?.Invoke(Connection);
                }
                catch (Exception ex)
                {
                    Error = ex;
                }
            }
            OnStatusChange?.Invoke();
        }

        internal ConnectionStatus GetConnectionStatus()
        {
            if (Configurations.Uri == null)
                return ConnectionStatus.NOT_CONFIGURED;
            else if (Error != null)
                return ConnectionStatus.ERROR;
            else if (Session == null)
                return ConnectionStatus.CONECTING;
            else
                return ConnectionStatus.CONNECTED;
        }

        private void Consumer_Listener(IMessage message) => OnMessageReciver?.Invoke(message);

        internal void OnObjectReciver<T>(MessageListener<T> a)
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
                        a.Invoke(obj);
                    }
                    catch (JsonSerializationException) { }
                }
            };
        }

        internal void SendMensege(string message)
        {
            IMessageProducer producer = Session.CreateProducer(Destination);
            var msg = producer.CreateTextMessage(message);
            producer.Send(msg);
        }

        internal void SendObject(object obj)
        {
            IMessageProducer producer = Session.CreateProducer(Destination);
            var strObj = JsonConvert.SerializeObject(obj);
            var msg = producer.CreateTextMessage(strObj);
            producer.Send(msg);
        }
    }

}
