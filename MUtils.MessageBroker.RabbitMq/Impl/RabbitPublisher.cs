using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MUtils.MessageBroker.Domain;
using MUtils.MessageBroker.Model;
using MUtils.MessageBroker.RabbitMq.Misc;
using RabbitMQ.Client;

namespace MUtils.MessageBroker.RabbitMq.Impl
{
    public class RabbitPublisher : IPublisher
    {
        private readonly IList<Queue> _queues = null;
        private readonly IList<Exchange> _exchange = null;
        private IModel _channel = null;
        private IConnection _connection = null;

        public RabbitPublisher(Server server)
        {
            Server = server ?? throw new NullReferenceException("Server Can't be null");

            _queues = new List<Queue>();
            _exchange = new List<Exchange>();

            Initialize();
        }

        public Server Server { get; } = null;

        public IEnumerable<Queue> Queues => null;

        public IEnumerable<Exchange> Exchanges => _exchange;

        public bool SendMessage(Exchange exchange, string routingKey, string message, MqProperties properties = null)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            return SendMessage(exchange, routingKey, buffer, properties);
        }

        public bool SendMessage(Queue queue, string message, MqProperties properties = null)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            return SendMessage(queue, buffer, properties);
        }

        public bool SendMessage(Exchange exchange, string routingKey, byte[] buffer, MqProperties properties = null)
        {
            try
            {
                _channel.BasicPublish(exchange.Name, routingKey, properties == null ? null : new RabbitProperties(properties), buffer);
                return true;
            }
            catch (Exception e)
            {
                //Log Exception
                return false;
            }
        }

        public bool SendMessage(Queue queue, byte[] buffer, MqProperties properties = null)
        {
            try
            {
                _channel.BasicPublish(string.Empty, queue.Name, properties == null ? null : new RabbitProperties(properties), buffer);
                return true;
            }
            catch (Exception e)
            {
                //Log Exception
                return false;
            }
        }

        public void DeclareQueue(Queue item)
        {
            if (item == null)
                throw new NullReferenceException("Queue Can't be null");
            if (string.IsNullOrEmpty(item.Name))
                throw new NullReferenceException("Queue Can't be null");

            var queueDeclareOk = _channel.QueueDeclare(item.Name, item.Durable, item.Exclusive, item.AutoDelete, item.Options);
            if (queueDeclareOk != null)
            {
                if (!_queues.Any(q => q.Name == queueDeclareOk.QueueName))
                    _queues.Add(item);
            }
        }

        public void DeclareExchange(Exchange item)
        {
            if (item == null)
                throw new NullReferenceException("Exchange can't be null");
            _channel.ExchangeDeclare(item.Name, item.Type, item.Durable, item.AutoDelete, item.Options);
            if (!_exchange.Any(e => e.Name == item.Name))
                _exchange.Add(item);
        }

        public void QueueBind(Queue queue, Exchange exchange, string routingKey, IDictionary<string, object> options = null)
        {
            _channel.QueueBind(queue.Name, exchange.Name, routingKey, options);
        }

        public void ExchangeBind(Exchange source, Exchange target, string routingKey, IDictionary<string, object> options = null)
        {
            _channel.ExchangeBind(target.Name, source.Name, routingKey, options);
        }

        public void DeleteQueue(Queue item)
        {
            _channel.QueueDelete(item.Name);
        }

        #region Private Methods
        private void Initialize()
        {
            RabbitConnectionFactory connectionFactory = new RabbitConnectionFactory();
            //{
            //    HostName = Server.HostName,
            //    UserName = Server.UserName,
            //    Password = Server.Password,
            //    VirtualHost = Server.VirtualHost
            //};
            _connection = connectionFactory.GetConnection(Server);
            _channel = _connection.CreateModel();
        }
        #endregion

        #region IDisposable Memberes
        public void Dispose()
        {
            Dispose(true);
        }

        private bool _disposed = false;
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                _channel.Close();
                _channel.Dispose();
                //_connection.Close();
                //_connection.Dispose();
            }
        }

        ~RabbitPublisher()
        {
            Dispose(false);
        }
        #endregion
    }
}