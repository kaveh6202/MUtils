using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MUtils.MessageBroker.Model;
using MUtils.MessageBroker.RabbitMq.Misc;
using RabbitMQ.Client;

namespace MUtils.MessageBroker.RabbitMq.Impl
{
    public class RabbitConnectionFactory //: ConnectionFactory
    {
        private readonly RabbitConnectionConfiguration _rabbitConfiguration;
        private readonly IEnumerable<IConnectionStrategy> _connectionStrategies;
        //private readonly Server _server;

        //public static GetConnectionInstance()

        //public RabbitConnectionFactory(IEnumerable<IConnectionStrategy> connectionStrategies)
        //{
        //    _connectionStrategies = connectionStrategies;
        //    //RequestedHeartbeat = heartbeat;
        //    _rabbitConfiguration = RabbitConnectionConfiguration.GetConfig();
        //}

        public RabbitConnectionFactory()
        {
            //_connectionStrategies = connectionStrategies;
            //RequestedHeartbeat = heartbeat;
            _rabbitConfiguration = RabbitConnectionConfiguration.GetConfig();
            _connectionStrategies = new List<IConnectionStrategy>()
            {
                new SingleConnectionStrategy(),
                new TtlConnectionStrategy(),
                new PerRequestConnectionStrategy()
            };
            //HostName = server.HostName;
            //UserName = server.UserName;
            //Password = server.Password;
            //VirtualHost = server.VirtualHost;
        }

        //private static readonly ConcurrentDictionary<string, IConnection> ConnectionPool = new ConcurrentDictionary<string, IConnection>();
        //private static readonly object CreateNewConnectionLock = new object();
        //private static readonly object ReOpenConnectionLock = new object();
        public IConnection GetConnection(Server server)
        {
            foreach (var connectionStrategy in _connectionStrategies)
            {
                if (connectionStrategy.CanHandle(_rabbitConfiguration.ConnectionType))
                {
                    return connectionStrategy.Handle(server, _rabbitConfiguration);
                }
            }
            return null;

            /*string key = $"{HostName}.{UserName}.{VirtualHost}";
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
            return connection;*/
        }
    }
}