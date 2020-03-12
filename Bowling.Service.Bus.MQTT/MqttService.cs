using Bowling.Domain.Game.Interfaces;
using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace Bowling.Service.Bus.MQTT
{
    public class MqttService : IBusService, IDisposable
    {
        private IManagedMqttClient _client;
        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<IBusService.ConnectionStatus> OnStatusChange;

        public IBusService.ConnectionStatus GetConnectionStatus()
        {
            throw new NotImplementedException();
        }

        public void OnObjectReciver<T>(Action<T> listener)
        {
        }

        public void SendText(string message)
        {
            _client.PublishAsync(new MqttApplicationMessageBuilder()
                    .WithTopic(MqttConfiguration.Topic)
                    .WithPayload(message)
                    .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel) 2)
                    .WithRetainFlag(false)
                    .Build())
                .Wait();
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
            var mqttUrl = MqttConfiguration.Host ?? "broker.mqttdashboard.com";
            var clientId = Guid.NewGuid().ToString();
            var mqttUser = MqttConfiguration.Username;
            var mqttPassword = MqttConfiguration.Password;
            var mqttPort = MqttConfiguration.Port;
            var mqttSecure = false;

            var messageBuilder = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                //.WithCredentials(mqttUser, mqttPassword)
                .WithTcpServer(mqttUrl, null)
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
            _client = new MqttFactory().CreateManagedMqttClient();
            await _client.StartAsync(managedOptions);

            _client.UseConnectedHandler(e => { Console.WriteLine("Connected successfully with MQTT Brokers."); });

            await _client.SubscribeAsync(new TopicFilterBuilder()
                .WithTopic(MqttConfiguration.Topic)
                .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel) 0)
                .Build());

            _client.UseApplicationMessageReceivedHandler(e =>
            {
                try
                {
                    string topic = e.ApplicationMessage.Topic;
                    if (string.IsNullOrWhiteSpace(topic) == false)
                    {
                        string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        Console.WriteLine($"Topic: {topic}. Message Received: {payload}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message, ex);
                }
            });

            await _client.PublishAsync(new MqttApplicationMessageBuilder()
                .WithTopic(MqttConfiguration.Topic)
                .WithPayload("oi")
                .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel) 2)
                .WithRetainFlag(false)
                .Build());
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