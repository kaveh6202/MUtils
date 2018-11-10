using System;
using System.Collections.Generic;
using MUtils.MessageBroker.Model;

namespace MUtils.MessageBroker.Domain
{
    public interface ISubscriber : IDisposable
    {
        bool AutoAck { get; }
        Server Server { get; }
        Queue Queue { get; }
        Action<Result<string>> Recievers { get; set; }
    }

    public interface ISubscriber<T> : ISubscriber
    {
        new Action<Result<T>> Recievers { get; set; }
    }
}