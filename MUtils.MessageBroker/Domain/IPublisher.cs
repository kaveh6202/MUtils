using System;
using System.Collections.Generic;
using MUtils.MessageBroker.Model;

namespace MUtils.MessageBroker.Domain
{
    public interface IPublisher : IDisposable
    {
        Server Server { get; }
        IEnumerable<Queue> Queues { get; }
        IEnumerable<Exchange> Exchanges { get; }
        bool SendMessage(Exchange exchange, string routingKey, string message, MqProperties properties = null);
        bool SendMessage(Queue queue, string message, MqProperties properties = null);
        bool SendMessage(Exchange exchange, string routingKey, byte[] buffer, MqProperties properties = null);
        bool SendMessage(Queue queue, byte[] buffer, MqProperties properties = null);

        void DeclareQueue(Queue item);
        void DeclareExchange(Exchange item);
        void QueueBind(Queue queue, Exchange exchange, string routingKey, IDictionary<string, object> options = null);
        void ExchangeBind(Exchange source, Exchange target, string routingKey, IDictionary<string, object> options = null);

        void DeleteQueue(Queue item);
    }
}