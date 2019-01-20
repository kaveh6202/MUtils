using System;
using MUtils.MessageBroker.Domain;
using MUtils.MessageBroker.Model;

namespace MUtils.MessageBroker
{
    public interface IMqProvider
    {
        IPublisher GetPublisher(Server server);
        ISubscriber<T> GetSubScriber<T>(Server server, Queue queue, Action<Result<T>> reciever, bool autoAck = false, Action shutDownAction = null);
        ISubscriber GetSubScriber(Server server, Queue queue, Action<Result<string>> reciever, bool autoAck = false, Action shutDownAction = null);
        IPullInfo<T> Pull<T>(Server server, Queue queue, ushort count, bool autoAck = true);
        IPullInfo<string> Pull(Server server, Queue queue, ushort count, bool autoAck = false);
        IPublisher GetOrNewPublisher(Server server, string publisherName);
        int GetMessageCount(Server server, Queue queue);
    }
}
