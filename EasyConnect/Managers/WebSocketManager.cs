using Fleck;
using System.Collections.Concurrent;

namespace EasyConnect.Managers
{
    public class WebSocketManager
    {
        private readonly ConcurrentDictionary<string, IWebSocketConnection> devicesConnected = [];
        private readonly ConcurrentDictionary<string, SemaphoreSlim> deviceLocks = [];

        public bool TryAdd(string id, IWebSocketConnection socket) 
            => devicesConnected.TryAdd(id, socket);
        public bool TryGet(string id, out IWebSocketConnection? socket)
            => devicesConnected.TryGetValue(id, out socket);
        public bool TryUpdate(string id, IWebSocketConnection oldSocket, IWebSocketConnection newSocket)
            => devicesConnected.TryUpdate(id, oldSocket, newSocket);
        public SemaphoreSlim GetDeviceLock(string id)
            => deviceLocks.GetOrAdd(
                id,
                _ => new SemaphoreSlim(1, 1));
        public IReadOnlyCollection<KeyValuePair<string, IWebSocketConnection>> Connections
            => devicesConnected;
        public void Clear()
            => devicesConnected.Clear();
        public void CloseConnections()
        {
            foreach (var device in devicesConnected)
            {
                device.Value.Close();
            }
            Clear();
        }
    }
}
