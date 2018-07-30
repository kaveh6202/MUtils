using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;
using MUtils.MessageBroker.Model;
using MUtils.MessageBroker.RabbitMq.Misc;
using RabbitMQ.Client;

namespace MUtils.MessageBroker.RabbitMq.Impl
{
    public sealed class TtlConnectionStrategy : ConnectionFactory,IConnectionStrategy
    {
        private static readonly ConcurrentDictionary<string, IConnection> ConnectionPool = new ConcurrentDictionary<string, IConnection>();
        private static readonly object CreateNewConnectionLock = new object();
        private static readonly object ReOpenConnectionLock = new object();
        private static readonly ObjectCache ConnectionPoolCache = System.Runtime.Caching.MemoryCache.Default;
        public bool CanHandle(ConnectionType type)
        {
            if (type == ConnectionType.PerInterval)
                return true;
            return false;
        }

        public IConnection Handle(Server server, RabbitConnectionConfiguration config)
        {
            HostName = server.HostName;
            Password = server.Password;
            VirtualHost = server.VirtualHost;
            UserName = server.UserName;
            RequestedHeartbeat = config.HeartBeat;

            string key = $"{HostName}.{UserName}.{VirtualHost}";
            var value = ConnectionPoolCache.Get(key);
            if (value != null)
                return value as IConnection;

            lock (CreateNewConnectionLock)
            {
                value = ConnectionPoolCache.Get(key);
                if (value == null)
                {
                    value = CreateConnection();
                    ConnectionPoolCache.Add(key, value,
                        new CacheItemPolicy() {SlidingExpiration = TimeSpan.FromSeconds(config.Ttl)});
                }
            }
            if (!(value as IConnection).IsOpen)
            {
                lock (ReOpenConnectionLock)
                {
                    value = ConnectionPoolCache.Get(key);
                    if (!(value as IConnection).IsOpen)
                    {
                        ConnectionPoolCache.Remove(key);
                        (value as IConnection).Dispose();
                        value = CreateConnection();
                        ConnectionPoolCache.Add(key, value,
                            new CacheItemPolicy() {SlidingExpiration = TimeSpan.FromSeconds(config.Ttl)});
                    }
                }
            }
            return value as IConnection;
        }
    }
}