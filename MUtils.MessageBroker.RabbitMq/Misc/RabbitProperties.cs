using MUtils.MessageBroker;
using RabbitMQ.Client.Framing;

namespace MUtils.MessageBroker.RabbitMq.Misc
{
    public class RabbitProperties : BasicProperties
    {
        public RabbitProperties(MqProperties properties)
        {
            DeliveryMode = properties.DeliveryMode;
            Persistent = properties.Persistent;
        }
    }
}