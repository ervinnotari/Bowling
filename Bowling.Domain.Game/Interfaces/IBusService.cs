using System;
using System.Threading.Tasks;

namespace Bowling.Domain.Game.Interfaces
{
    public interface IBusService : IDisposable
    {
        public struct ConnectionInfo
        {
            public ConnectionInfo(Uri uri, string topic)
            {
                BrokerUri = uri;
                Topic = topic;
            }

            public Uri BrokerUri { get; }
            public string Topic { get; }
        }
        public enum ConnectionStatus { Disabled, Connected, Conecting, Error }

        public event Action<object> OnMessageReciver;
        public event Action<object> OnConnection;
        public event Action<ConnectionStatus, ConnectionInfo> OnStatusChange;

        public ConnectionStatus GetConnectionStatus();
        public void OnObjectReciver<T>(Action<T> listener);
        public void SendText(string message);
        public void SendObject(object obj);

        public void ConnectionStart();
        public void ConnectionStart(Uri uri);
        public Task ConnectionStartAsync();
        public Task ConnectionStartAsync(Uri uri);

        public void ConnectionStop();
        public Task ConnectionStopAsync();

        public Exception GetError();
    }
}
