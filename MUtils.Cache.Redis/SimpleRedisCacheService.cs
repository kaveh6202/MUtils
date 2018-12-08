using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUtils.Interface;
using MUtils.Interface.ConfigurationModel;
using Sider;


namespace MUtils.Cache.Redis
{
    public class SimpleRedisCacheService : IRedisCacheService, ICacher
    {
        private readonly RedisClient _dataBase = null;
        private readonly ISerializer _serializer = null;
        private readonly ILogger _logger = null;
        private readonly RedisConfiguration _config = null;
        public SimpleRedisCacheService(RedisConfiguration configuration, ISerializer serializer
                               , IConnectionFactory connectionFactory = null, ILogger logger = null
                               , int db = 0)
        {
            _config = configuration;
            _logger = logger;
            var connFactory = connectionFactory ?? new RedisDefaultConnectionFactory();
            _dataBase = connFactory.GetSimpleConnection(configuration);
        }

        public SimpleRedisCacheService(RedisConfiguration configuration, int db = 0)
        {
            _config = configuration;
            var connFactory = new RedisDefaultConnectionFactory();
            _dataBase = connFactory.GetSimpleConnection(configuration);
        }

        public object GetValue(string key)
        {
            var value = _dataBase.Get(key);
            return value;
        }

        public IDictionary<string, object> GetValues(IList<string> keys)
        {
            int length = keys.Count;
            var redisValues = _dataBase.MGet(keys.ToArray());
            IDictionary<string, object> result = new Dictionary<string, object>(length);
            for (int i = 0; i < length; i++)
                result.Add(keys[i], redisValues[i]);
            return result;
        }

        public void SetValue(string key, object value, CacheConfiguration policy)
        {
            throw new NotImplementedException();
        }

        public void SetValues(Dictionary<string, object> keyValues, CacheConfiguration policy)
        {
            throw new NotImplementedException();
        }

        public void Delete(string key)
        {
            throw new NotImplementedException();
        }

        public void Delete(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string key)
        {
            throw new NotImplementedException();
        }

        public string GetString(string key, string region = null)
        {
            var value = _dataBase.Get(key);
            return value;
        }

        public async Task<string> GetStringAsync(string key, string region = null)
        {
            throw new NotImplementedException();
        }

        public string[] GetStrings(string[] keys, string region = null)
        {
            var length = keys.Length;
            var redisValues = _dataBase.MGet(keys.ToArray());
            var values = new string[length];
            for (var i = 0; i < length; i++)
                values[i] = redisValues[i];
            return values;
        }

        public async Task<string[]> GetStringsAsync(string[] keys, string region = null)
        {
            throw new NotImplementedException();
        }

        public T GetValue<T>(string key, string region = null)
        {
            var value = _dataBase.Get(key);
            return Deserialize<T>(value);
        }

        public async Task<T> GetValueAsync<T>(string key, string region = null)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, T> GetValues<T>(string[] keys)
        {
            int length = keys.Length;
            var redisValues = _dataBase.MGet(keys.ToArray());
            IDictionary<string, T> result = new Dictionary<string, T>(length);
            for (int i = 0; i < length; i++)
                result.Add(keys[i], Deserialize<T>(redisValues[i]));
            return result;
        }

        public async Task<IDictionary<string, T>> GetValuesAsync<T>(string[] keys)
        {
            int length = keys.Length;
            var redisValues = _dataBase.MGet(keys);
            IDictionary<string, T> result = new Dictionary<string, T>(length);
            for (int i = 0; i < length; i++)
                result.Add(keys[i], Deserialize<T>(redisValues[i]));
            return result;
        }

        public void SetString(string key, string value, TimeSpan lifeTime)
        {
            _dataBase.SetEX(key, lifeTime, value);
        }
        

        public void SetString(string key, string value)
        {
            _dataBase.Set(key, value);
        }

        public void SetStrings(Dictionary<string, string> keyValues)
        {
            var kv = keyValues.ToDictionary(o => o.Key, o => o.Value).ToArray();
            var resp = _dataBase.MSet(kv);
            if (!resp)
            {
                throw new Exception("batch insertion encoured an error");
            }
        }

        public async Task SetStringAsync(string key, string value, TimeSpan lifeTime)
        {
            throw new NotImplementedException();
        }

        public async Task SetStringAsync(string key, string value)
        {
            throw new NotImplementedException();
        }

        public async Task SetStringsAsync(Dictionary<string, string> keyValues)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string key, object value, TimeSpan lifeTime)
        {
            var json = _serializer.Serialize(value);
            _dataBase.SetEX(key, lifeTime, json);
        }
        public void SetValue(string key, object value)
        {
            var json = _serializer.Serialize(value);
            _dataBase.Set(key, json);
        }

        public async Task SetValueAsync(string key, object value, TimeSpan lifeTime)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string key, object value, DateTimeOffset expireTime)
        {
            var json = _serializer.Serialize(value);
            _dataBase.SetEX(key, expireTime - DateTimeOffset.UtcNow, json);
        }

        public async Task SetValueAsync(string key, object value, DateTimeOffset expireTime)
        {
            //var json = _serializer.Serialize(value);
            //await _dataBase.StringSetAsync(key, json, expireTime - DateTimeOffset.UtcNow);
        }

        #region Private Method
        private T Deserialize<T>(string value)
        {
            if (string.IsNullOrEmpty(value))
                return default(T);
            try
            {
                return _serializer.Deserialize<T>(value);
            }
            catch (Exception e)
            {
                _logger?.LogError(GetType().Name, e.Message, e);
                return default(T);
            }
        }
        #endregion

        #region IDisposable Members
        private bool _disposed = false;
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                //Dispose Managed Resources
            }

            //Dispose Unmanaged Resources

            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
        }

        ~SimpleRedisCacheService()
        {
            Dispose(false);
        }
        #endregion
    }
}
