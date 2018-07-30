using System.Collections.Concurrent;
using MUtils.MessageBroker.Model;
using MUtils.MessageBroker.RabbitMq.Misc;
using RabbitMQ.Client;

namespace MUtils.MessageBroker.RabbitMq.Impl
{
    public sealed class SingleConnectionStrategy : ConnectionFactory,IConnectionStrategy
    {
        private static readonly ConcurrentDictionary<string, IConnection> ConnectionPool = new ConcurrentDictionary<string, IConnection>();
        private static readonly object CreateNewConnectionLock = new object();
        private static readonly object ReOpenConnectionLock = new object();
        public bool CanHandle(ConnectionType type)
        {
            if (type == ConnectionType.Single)
                return true;
            return false;
        }

        public IConnection Handle(Server server,RabbitConnectionConfiguration config)
        {
            HostName = server.HostName;
            Password = server.Password;
            VirtualHost = server.VirtualHost;
            UserName = server.UserName;
            RequestedHeartbeat = config.HeartBeat;

            string key = $"{HostName}.{UserName}.{VirtualHost}";
            ConnectionPool.TryGetValue(key, out IConnection connection);
            if (connection == null)
            {
                lock (CreateNewConnectionLock)
                {
                    ConnectionPool.TryGetValue(key, out connection);
                    if (connection == null)
                    {
                        connection = CreateConnection();
                        ConnectionPool.TryAdd(key, connection);
                    }
                }
            }
            if (!connection.IsOpen)
            {
                lock (ReOpenConnectionLock)
                {
                    ConnectionPool.TryGetValue(key, out connection);
                    if (!connection.IsOpen)
                    {
                        connection.Close();
                        connection.Dispose();
                        connection = CreateConnection();
                        ConnectionPool.TryAdd(key, connection);
                    }
                }
            }
            return connection;
        }
    }
}