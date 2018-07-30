using System;
using System.Collections.Generic;
using MUtils.MessageBroker.Model;

namespace MUtils.MessageBroker.Domain
{
    public interface IPullInfo<T> : IDisposable
    {
        IEnumerable<Result<T>> ResultSet { get; }
    }
}