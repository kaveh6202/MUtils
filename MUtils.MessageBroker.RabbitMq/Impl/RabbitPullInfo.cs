using System.Collections.Generic;
using MUtils.MessageBroker.Domain;
using MUtils.MessageBroker.Model;
using RabbitMQ.Client;

namespace MUtils.MessageBroker.RabbitMq.Impl
{
    public class RabbitPullInfo<T> : IPullInfo<T>
    {
        private readonly IConnection _connection = null;
        private readonly IModel _channel = null;
        public RabbitPullInfo(IConnection connection, IModel channel, IEnumerable<Result<T>> resultSet)
        {
            _connection = connection;
            _channel = channel;
            ResultSet = resultSet;
        }

        public IEnumerable<Result<T>> ResultSet { get; private set; }

        #region IDisposable Members
        private bool _disposed = false;
        public void Dispose()
        {
            Dispose(true);
        }

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

        ~RabbitPullInfo()
        {
            Dispose(false);
        }
        #endregion
    }
}