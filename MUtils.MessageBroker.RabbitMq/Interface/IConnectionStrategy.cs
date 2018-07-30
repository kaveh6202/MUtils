using MUtils.MessageBroker.Model;
using MUtils.MessageBroker.RabbitMq.Misc;
using RabbitMQ.Client;

namespace MUtils.MessageBroker.RabbitMq.Impl
{
    public interface IConnectionStrategy
    {
        bool CanHandle(ConnectionType type);
        IConnection Handle(Server server, RabbitConnectionConfiguration config);
    }
}