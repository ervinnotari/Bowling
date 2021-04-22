using System;
using System.Threading.Tasks;

namespace Bowling.Domain.Game.Interfaces
{
    public interface IBusService : IDisposable
    {
        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<ConnectionStatus, object> OnStatusChange;

        public enum ConnectionStatus { Disabled, Connected, Conecting, Error }
        public ConnectionStatus GetConnectionStatus();
        public void OnObjectReciver<T>(Action<T> listener);
        public void SendText(string message);
        public void SendObject(object obj);

        public void ConnectionStop();
        public Task ConnectionStopAsync();

        public void ConnectionStart();
        public Task ConnectionStartAsync();
        public Task ConnectionStartAsync(string host, int portc);
        public Task ConnectionStartAsync(string host, int port, string username, string password);
        public void ConnectionStart(string host, int port);
        public void ConnectionStart(string host, int port, string username, string password);

        public Exception GetError();
    }
}
