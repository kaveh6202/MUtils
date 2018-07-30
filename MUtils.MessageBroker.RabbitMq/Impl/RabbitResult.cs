using MUtils.MessageBroker.Model;
using RabbitMQ.Client;

namespace MUtils.MessageBroker.RabbitMq.Impl
{
    public class RabbitResult<T> : Result<T>
    {
        public RabbitResult(T data, ulong deliveryTag, IModel channel)
            : base(data)
        {
            DeliveryTag = deliveryTag;
            Channel = channel;
        }

        internal ulong DeliveryTag { get; private set; }

        internal IModel Channel { get; private set; }

        public override void Ack(bool multipel = false)
        {
            Channel.BasicAck(DeliveryTag, multipel);
        }

        public override void Nack(bool multipel = false, bool requeue = true)
        {
            if (Channel.IsOpen)
                Channel.BasicNack(DeliveryTag, multipel, requeue);
        }
    }
}