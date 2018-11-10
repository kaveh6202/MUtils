using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Threading.Tasks;
using MUtils.Interface;
using MUtils.Interface.ConfigurationModel;

namespace MUtils.MemoryCache
{
    public class MemoryCacheService : ICacher
    {
        private readonly ObjectCache _cache = System.Runtime.Caching.MemoryCache.Default;
        private readonly IObjectMapper _mapper;

        public MemoryCacheService(IObjectMapper mapper)
        {
            _mapper = mapper;
        }

        public object GetValue(string key)
        {
            return _cache.Get(key);
        }

        public IDictionary<string, object> GetValues(IList<string> keys)
        {
            return _cache.GetValues(keys);
        }

        public void SetValue(string key, object value, CacheConfiguration policy)
        {
            var cachePolicy = _mapper.Map<CacheItemPolicy>(policy);
            _cache.Set(key, value, cachePolicy);
        }

        public void SetValues(Dictionary<string, object> keyValues, CacheConfiguration policy)
        {
            var cachePolicy = _mapper.Map<CacheItemPolicy>(policy);
            foreach (var keyValue in keyValues)
            {
                _cache.Set(keyValue.Key, keyValue.Value, cachePolicy);
            }
        }

        public async Task<IDictionary<string, string>> GetValuesAsync(IList<string> keys)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            _cache.Remove(key);
        }

        public void Delete(IEnumerable<string> keys)
        {
            foreach (var key in keys)
            {
                _cache.Remove(key);
            }
        }
    }
}
