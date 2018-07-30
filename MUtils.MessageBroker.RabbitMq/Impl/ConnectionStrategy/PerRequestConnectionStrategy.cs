using MUtils.MessageBroker.Model;
using MUtils.MessageBroker.RabbitMq.Misc;
using RabbitMQ.Client;

namespace MUtils.MessageBroker.RabbitMq.Impl
{
    public class PerRequestConnectionStrategy : ConnectionFactory,IConnectionStrategy
    {
        public bool CanHandle(ConnectionType type)
        {
            if (type == ConnectionType.PerRequest)
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

            return CreateConnection();
        }
    }
}