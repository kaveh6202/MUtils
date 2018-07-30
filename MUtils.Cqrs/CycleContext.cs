using System.Collections.Concurrent;

namespace MUtils.Cqrs {

    public class CycleContext : ICycleContext {

        private readonly ConcurrentDictionary<string, object>
            _cache = new ConcurrentDictionary<string, object>();

        /// <inheritdoc />
        public void AddItem(string key, object value) {
            _cache.AddOrUpdate(key, k => value, (k, o) => value);
        }

        /// <inheritdoc />
        public object GetItem(string key) {
            object obj;
            if (_cache.TryGetValue(key, out obj))
                return obj;
            return null;
        }

    }
}