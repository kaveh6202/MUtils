using System;
using MUtils.MessageBroker.Domain;
using MUtils.MessageBroker.Model;

namespace MUtils.MessageBroker
{
    public interface IMqProvider
    {
        IPublisher GetPublisher(Server server);
        ISubscriper<T> GetSubScriber<T>(Server server, Queue queue, Action<Result<T>> reciever, bool autoAck = false, Action shutDownAction = null);
        IPullInfo<T> Pull<T>(Server server, Queue queue, ushort count, bool autoAck = true);
        IPublisher GetOrNewPublisher(Server server, string publisherName);
    }
}
