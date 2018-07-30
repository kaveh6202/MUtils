using System;

namespace MUtils.Cqrs {
    public interface ICacheService {
        object Get(string key, CacheScope scope = CacheScope.Default);
        void Set(string key, object value, DateTimeOffset expiry, CacheScope scope = CacheScope.Default);
    }
}