using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using MUtils.Interface;
using MUtils.MessageBroker.Domain;
using MUtils.MessageBroker.Model;
using MUtils.MessageBroker.RabbitMq.Impl;
using RabbitMQ.Client;

namespace MUtils.MessageBroker.RabbitMq
{
    public class RabbitMqProvider : IMqProvider
    {
        private readonly ISerializer _serializer = null;
        private static readonly ConcurrentDictionary<string, IPublisher> PublisherPool = new ConcurrentDictionary<string, IPublisher>();
        public RabbitMqProvider(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public IPublisher GetPublisher(Server server)
        {
            return new RabbitPublisher(server);
        }

        public ISubscriber<T> GetSubScriber<T>(Server server, Queue queue, Action<Result<T>> reciever, bool autoAck = false,
            Action shutDownAction = null)
        {
            return new RabbitSubScriber<T>(_serializer, server, queue, reciever, autoAck, shutDownAction);
        }

        public ISubscriber GetSubScriber(Server server, Queue queue, Action<Result<string>> reciever, bool autoAck = false,
            Action shutDownAction = null)
        {
            return new RabbitSubScriber(_serializer, server, queue, reciever, autoAck, shutDownAction);
        }

       

        public IPublisher GetOrNewPublisher(Server server, string publisherName)
        {
            return PublisherPool.GetOrAdd(publisherName, GetPublisher(server));
        }

        public int GetMessageCount(Server server, Queue queue)
        {
            throw new NotImplementedException();
        }

        public ISubscriber<T> GetSubScriber<T>(Server server, Queue queue, Action<Result<T>> reciever, bool autoAck)
        {
            return new RabbitSubScriber<T>(_serializer, server, queue, reciever, autoAck);
        }

        public IPullInfo<T> Pull<T>(Server server, Queue queue, ushort count, bool autoAck = true)
        {
            //IList<Result<T>> result = new List<Result<T>>();

            ConcurrentBag<Result<T>> result = new ConcurrentBag<Result<T>>();
            if (server == null)
                throw new NullReferenceException("Server cann't be null");
            if (queue == null)
                throw new NullReferenceException("Queue cann't be null");

            RabbitConnectionFactory connectionFactory = new RabbitConnectionFactory();
            IConnection connection = connectionFactory.GetConnection(server);

            IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete, queue.Options);
            Parallel.For(0, count, (index, loopState) =>
            {
                BasicGetResult queueResult = channel.BasicGet(queue.Name, autoAck);
                if (queueResult == null)
                {
                    loopState.Break();
                    return;
                }
                string json = Encoding.UTF8.GetString(queueResult.Body);
                var data = _serializer.Deserialize<T>(json);
                RabbitResult<T> item = new RabbitResult<T>(data, queueResult.DeliveryTag, channel);
                result.Add(item);
            });
            return new RabbitPullInfo<T>(connection, channel, result);
        }
        public IPullInfo<string> Pull(Server server, Queue queue, ushort count, bool autoAck = false)
        {
            //IList<Result<T>> result = new List<Result<T>>();

            ConcurrentBag<Result<string>> result = new ConcurrentBag<Result<string>>();
            if (server == null)
                throw new NullReferenceException("Server cann't be null");
            if (queue == null)
                throw new NullReferenceException("Queue cann't be null");

            RabbitConnectionFactory connectionFactory = new RabbitConnectionFactory();
            IConnection connection = connectionFactory.GetConnection(server);

            IModel channel = connection.CreateModel();
            channel.QueueDeclare(queue.Name, queue.Durable, queue.Exclusive, queue.AutoDelete, queue.Options);
            Parallel.For(0, count, (index, loopState) =>
            {
                BasicGetResult queueResult = channel.BasicGet(queue.Name, autoAck);
                if (queueResult == null)
                {
                    loopState.Break();
                    return;
                }
                string json = Encoding.UTF8.GetString(queueResult.Body);
                RabbitResult<string> item = new RabbitResult<string>(json, queueResult.DeliveryTag, channel);
                result.Add(item);
            });
            return new RabbitPullInfo<string>(connection, channel, result);
        }
    }
}
