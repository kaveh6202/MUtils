using System;
using MUtils.MessageBroker.Model;

namespace MUtils.MessageBroker.RabbitMq.Misc
{
    public class RabbitConnectionConfiguration
    {
        public long Ttl;
        public ConnectionType ConnectionType;
        public ushort HeartBeat;
        public Qos Qos;
        private static Lazy<RabbitConnectionConfiguration> _lazyValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionType"></param>
        /// <param name="heartBeat"></param>
        /// <param name="ttl">only available when connectionType.PerInterval is used</param>
        /// <param name="qos"></param>
        /// <returns></returns>
        public static RabbitConnectionConfiguration CreateConfig(ConnectionType connectionType = ConnectionType.Single,
            ushort heartBeat = 15, long ttl = 0, Qos qos = null)
        {
            _lazyValue = new Lazy<RabbitConnectionConfiguration>(() =>
                new RabbitConnectionConfiguration(connectionType, heartBeat, ttl, qos ?? new Qos()));
            return _lazyValue.Value;
        }

        public static RabbitConnectionConfiguration GetConfig()
        {
            return _lazyValue.IsValueCreated ? _lazyValue.Value : CreateConfig();
        }
        private RabbitConnectionConfiguration(ConnectionType connectionType,ushort heartBeat,long ttl,Qos qos)
        {
            ConnectionType = connectionType;
            Ttl = ttl;
            HeartBeat = heartBeat;
            this.Qos = qos;
        }
    }

    public enum ConnectionType
    {
        Single,
        PerRequest,
        PerInterval
    }
}