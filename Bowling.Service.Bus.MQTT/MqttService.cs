using Bowling.Domain.Game.Interfaces;
using Bowling.Domain.Game.Utils;
using System;
using System.Threading.Tasks;

namespace Bowling.Service.Bus.MQTT
{
    public class MqttService : IBusService
    {
        public BusConfiguration Configuration { get; set; }

        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<IBusService.ConnectionStatus> OnStatusChange;

        public MqttService(BusConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConnectionStart()
        {

        }

        public Task ConnectionStartAsync()
        {
            return Task.CompletedTask;
        }

        public IBusService.ConnectionStatus GetConnectionStatus()
        {
            return IBusService.ConnectionStatus.ERROR;
        }

        public Exception GetError()
        {
            return null;
        }

        public void OnObjectReciver<T>(Action<T> listener)
        {

        }

        public void SendObject(object obj)
        {

        }

        public void SendText(string message)
        {

        }
    }
}
