using Bowling.Domain.Game.Interfaces;
using System;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace Bowling.Service.Bus.MQTT
{
    public class MqttService : IBusService, IDisposable
    {
        public MqttConfiguration Configuration { get; set; }

        public MqttService(MqttConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IManagedMqttClient client;
        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<IBusService.ConnectionStatus> OnStatusChange;
        public IBusService.ConnectionStatus GetConnectionStatus()
        {
            throw new NotImplementedException();
        }

        public void OnObjectReciver<T>(Action<T> listener)
        {
            throw new NotImplementedException();
        }

        public void SendText(string message)
        {
            throw new NotImplementedException();
        }

        public void SendObject(object obj)
        {
            client.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic(Configuration.Topic)
                .WithPayload("teste")
                .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)0)
                .WithRetainFlag(false)
                .Build())
                .Wait();
        }

        public void ConnectionStart()
        {
            ConnectionStartAsync().Wait();
        }

        public async Task ConnectionStartAsync()
        {
            string mqttURI = "broker.mqttdashboard.com";
            string clientId = Guid.NewGuid().ToString();
            string mqttUser = "";
            string mqttPassword = "";
            int mqttPort = 1883;
            bool mqttSecure = false;
                
            var messageBuilder = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithCredentials(mqttUser, mqttPassword)
                .WithTcpServer(mqttURI, mqttPort)
                .WithCleanSession();
            var options = mqttSecure
                ? messageBuilder
                    .WithTls()
                    .Build()
                : messageBuilder
                    .Build();
            var managedOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(options)
                .Build();
            client = new MqttFactory().CreateManagedMqttClient();
            await client.StartAsync(managedOptions);
        }

        public Exception GetError()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}