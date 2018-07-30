using System;

namespace MUtils.CqrsBase {
    public interface IQuery
    {
        Guid SessionId { get;}
    }
    public interface IQuery<TResult> : IQuery { }

    public interface ICachableQuery<TResult> : IQuery<TResult> {
        string CacheKey { get; }
        TimeSpan LifeSpan { get; }
    }
}