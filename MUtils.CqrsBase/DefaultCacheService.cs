using System;
using System.Runtime.Caching;

namespace MUtils.CqrsBase {

    public class DefaultCacheService : ICacheService
    {

        private static readonly MemoryCache MemoryCache = MemoryCache.Default;
        private readonly ICycleContext _cycleContext;

        public DefaultCacheService(ICycleContext cycleContext) {
            _cycleContext = cycleContext;
        }

        /// <inheritdoc />
        public object Get(string key, CacheScope scope = CacheScope.Default) {
            switch (scope) {
                case CacheScope.Default:
                    return MemoryCache.Get(key);
                case CacheScope.CurrentRequest:
                    return _cycleContext.GetItem(key);
                case CacheScope.UserSession:
                default:
                    throw new NotSupportedException();
            }
        }

        /// <inheritdoc />
        public void Set(string key, object value, DateTimeOffset expiry, CacheScope scope = CacheScope.Default) {
            switch (scope) {
                case CacheScope.Default:
                    MemoryCache.Set(key, value, expiry);
                    break;
                case CacheScope.CurrentRequest:
                    _cycleContext.AddItem(key, value);
                    break;
                case CacheScope.UserSession:
                default:
                    throw new NotSupportedException();
            }
        }
    }
}