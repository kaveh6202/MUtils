using System;
using System.Collections.Generic;
using MUtils.MessageBroker.Model;

namespace MUtils.MessageBroker.Domain
{
    public interface ISubscriper<T> : IDisposable
    {
        bool AutoAck { get; }
        Server Server { get; }
        Queue Queue { get; }
        Action<Result<T>> Recievers { get; set; }
    }
}