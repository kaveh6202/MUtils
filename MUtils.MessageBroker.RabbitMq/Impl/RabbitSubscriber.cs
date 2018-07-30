using System;
using System.Text;
using MUtils.MessageBroker.Domain;
using MUtils.MessageBroker.Model;
using MUtils.MessageBroker.RabbitMq.Misc;
using MUtils.Interface;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MUtils.MessageBroker.RabbitMq.Impl
{
    public class RabbitSubScriber<T> : ISubscriper<T>
    {
        private readonly Server _server = null;
        private readonly Queue _queue = null;
        //private readonly Qos _qos = null;
        private readonly bool _autoAck = false;
        private readonly ISerializer _serializer = null;
        private IConnection _connection = null;
        private IModel _channel = null;
        private EventingBasicConsumer _consumer = null;

        public RabbitSubScriber(ISerializer serializer, Server server, Queue queue, Action<Result<T>> reciever, bool autoAck = false , Action shutDownAction = null)
        {
            _server = server ?? throw new NullReferenceException("Server cann't be null");
            _queue = queue ?? throw new NullReferenceException("Queue cann't be null");
            _serializer = serializer;

            if (reciever != null)
                Recievers += reciever;
            //_qos = qos ?? new Qos();
            if (shutDownAction != null)
                ShutDownAction += shutDownAction;
            Initialize();
        }

        public bool AutoAck => _autoAck;

        public Server Server => _server;

        public Queue Queue => _queue;

        public Qos Qos => RabbitConnectionConfiguration.GetConfig().Qos;

        public Action<Result<T>> Recievers { get; set; }
        public Action ShutDownAction { get; set; }

        #region PrivateMethods
        private void Initialize()
        {
            RabbitConnectionFactory connectionFactory = new RabbitConnectionFactory();

            _connection = connectionFactory.GetConnection(_server);
            _channel = _connection.CreateModel();
            _channel.BasicQos((uint)Qos.PreFetchSize, (ushort)Qos.PreFetchCount, Qos.Global);
            _channel.QueueDeclare(_queue.Name, _queue.Durable, _queue.Exclusive, _queue.AutoDelete, _queue.Options);

            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += ConsumerReceived;
            _consumer.Shutdown += ConsumerShutdown;

            //TODO: Check it
            var result = _channel.BasicConsume(_queue.Name, _autoAck, _consumer);
        }

        private void ConsumerShutdown(object sender, ShutdownEventArgs e)
        {
            ShutDownAction.Invoke();
        }

        private void ConsumerReceived(object sender, BasicDeliverEventArgs e)
        {
            string jsonData = Encoding.UTF8.GetString(e.Body);
            RabbitResult<T> item = new RabbitResult<T>(_serializer.Deserialize<T>(jsonData), e.DeliveryTag, _channel);
            Recievers.Invoke(item);
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _consumer.HandleBasicCancelOk(_consumer.ConsumerTag);
                _consumer = null;
                _channel.Close(0, "Publisher Disposed");
                _channel.Dispose();
            }
        }

        ~RabbitSubScriber()
        {
            Dispose(false);
        }
        #endregion
    }
}