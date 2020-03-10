using System;
using System.Threading.Tasks;

namespace Bowling.Domain.Game.Interfaces
{
    public interface IAmqpService
    {
        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<IAmqpService.ConnectionStatus> OnStatusChange;

        public enum ConnectionStatus { DISABLED, CONNECTED, CONECTING, ERROR }
        public ConnectionStatus GetConnectionStatus();
        public void OnObjectReciver<T>(Action<T> listener);
        public void SendText(string message);
        public void SendObject(object obj);
        public void ConnectionStart();
        public Task ConnectionStartAsync();
        public Exception GetError();
    }
}
