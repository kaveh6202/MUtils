using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MUtils.CqrsBase {

    public class QueryCacheDecorator<TQuery, TResult>
        : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult> {

        private readonly ICacheService _cacheService;
        private readonly IQueryHandler<TQuery, TResult> _decoratee;

        public QueryCacheDecorator(ICacheService cacheService, IQueryHandler<TQuery, TResult> decoratee) {
            _cacheService = cacheService;
            _decoratee = decoratee;
        }

        public TResult Handle(TQuery query) {

            var attr = query.GetType().GetCustomAttribute<CachePolicyAttribute>(true);
            if (attr == null)
                return _decoratee.Handle(query);

            var cacheKey = GetCacheKey(attr, query);
            var obj = _cacheService.Get(cacheKey, attr.Scope);
            if (obj is TResult)
                return (TResult)obj;
            var result = _decoratee.Handle(query);
            if (result != null)
                _cacheService.Set(cacheKey, result, 
                    DateTimeOffset.UtcNow.Add(TimeSpan.FromSeconds(attr.LifeSpanInSecond)));
            return result;
        }

        private string GetCacheKey(CachePolicyAttribute attr, TQuery query) {
            switch (attr.KeyPolicy) {
                case CacheKeyPolicy.GetType:
                    return query.GetType().FullName;
                case CacheKeyPolicy.Serialize:
                    return CqrsConfig.Default.Serializer.Invoke(query);
                case CacheKeyPolicy.HasKey:
                    return GetCacheKeyFromName(attr.KeyPropertyName, query);
                default:
                    throw new InvalidOperationException();
            }
        }

        private string GetCacheKeyFromName(string propertyName, TQuery query) {
            var prop = query.GetType().GetProperty(propertyName);
            if (prop == null) throw new InvalidOperationException();
            var value = prop.GetValue(query) as string;
            return value;
        }

        public Task<TResult> HandleAsync(TQuery query) {
            // لطفا این هم پیاده بشه
            throw new NotImplementedException();
        }
    }
}