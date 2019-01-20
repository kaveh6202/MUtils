using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MUtils.Interface;
using MUtils.Interface.ConfigurationModel;
using StackExchange.Redis;

namespace MUtils.Cache.Redis
{
    public class RedisCacheService : IRedisCacheService, ICacher
    {
        private readonly IDatabase _dataBase = null;
        private readonly ISerializer _serializer = null;
        private readonly ILogger _logger = null;
        private readonly RedisConfiguration _config = null;
        public RedisCacheService(RedisConfiguration configuration, ISerializer serializer
                               , IConnectionFactory connectionFactory = null, ILogger logger = null
                               , int db = 0)
        {
            _config = configuration;
            _logger = logger;
            var connFactory = connectionFactory ?? new RedisDefaultConnectionFactory();
            //_dataBase = Task.Run(async () => await connFactory.GetDataBase(configuration, db)).Result;
            _dataBase = connFactory.GetDataBase(configuration, db).Result;
        }

        public RedisCacheService(RedisConfiguration configuration, int db = 0)
        {
            _config = configuration;
            var connFactory = new RedisDefaultConnectionFactory();
            _dataBase = Task.Run(async () => await connFactory.GetDataBase(configuration, db)).Result;
        }

        public object GetValue(string key)
        {
            var value = _dataBase.StringGet(key);
            return value.HasValue ? (object) value : null;
        }

        public IDictionary<string, object> GetValues(IList<string> keys)
        {
            int length = keys.Count;
            RedisKey[] redisKeys = new RedisKey[length];
            for (int i = 0; i < length; i++)
                redisKeys[i] = keys[i];
            RedisValue[] redisValues = _dataBase.StringGet(redisKeys);
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
            _dataBase.KeyDelete(key);
        }

        public void Delete(IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(string key)
        {
            await _dataBase.KeyDeleteAsync(key);
        }

        public string GetString(string key, string region = null)
        {
            var value = _dataBase.StringGet(key);
            return value.HasValue ? (string)value : null;
        }

        public async Task<string> GetStringAsync(string key, string region = null)
        {
            var value = await _dataBase.StringGetAsync(key);
            return value.HasValue ? (string)value : null;
        }

        public string[] GetStrings(string[] keys, string region = null)
        {
            var length = keys.Length;
            var redisKeys = new RedisKey[length];
            for (var i = 0; i < length; i++)
                redisKeys[i] = keys[i];
            var redisValues = _dataBase.StringGet(redisKeys);
            var values = new string[length];
            for (var i = 0; i < length; i++)
                values[i] = redisValues[i];
            return values;
        }

        public async Task<string[]> GetStringsAsync(string[] keys, string region = null)
        {
            var length = keys.Length;
            var redisKeys = new RedisKey[length];
            for (var i = 0; i < length; i++)
                redisKeys[i] = keys[i];
            var redisValues = await _dataBase.StringGetAsync(redisKeys);
            var values = new string[length];
            for (var i = 0; i < length; i++)
                values[i] = redisValues[i];
            return values;
        }

        public T GetValue<T>(string key, string region = null)
        {
            var value = _dataBase.StringGet(key);
            return Deserialize<T>(value);
        }

        public async Task<T> GetValueAsync<T>(string key, string region = null)
        {
            var value = await _dataBase.StringGetAsync(key);
            return Deserialize<T>(value);
        }

        public IDictionary<string, T> GetValues<T>(string[] keys)
        {
            int length = keys.Length;
            RedisKey[] redisKeys = new RedisKey[length];
            for (int i = 0; i < length; i++)
                redisKeys[i] = keys[i];
            RedisValue[] redisValues = _dataBase.StringGet(redisKeys);
            IDictionary<string, T> result = new Dictionary<string, T>(length);
            for (int i = 0; i < length; i++)
                result.Add(keys[i], Deserialize<T>(redisValues[i]));
            return result;
        }

        public async Task<IDictionary<string, T>> GetValuesAsync<T>(string[] keys)
        {
            int length = keys.Length;
            RedisKey[] redisKeys = new RedisKey[length];
            for (int i = 0; i < length; i++)
                redisKeys[i] = keys[i];
            RedisValue[] redisValues = await _dataBase.StringGetAsync(redisKeys);
            IDictionary<string, T> result = new Dictionary<string, T>(length);
            for (int i = 0; i < length; i++)
                result.Add(keys[i], Deserialize<T>(redisValues[i]));
            return result;
        }

        public void SetString(string key, string value, TimeSpan lifeTime)
        {
            _dataBase.StringSet(key, value, lifeTime);
        }
        

        public void SetString(string key, string value)
        {
            _dataBase.StringSet(key, value);
        }

        public void SetStrings(Dictionary<string, string> keyValues)
        {
            var kv = keyValues.ToDictionary(o => (RedisKey)o.Key, o => (RedisValue)o.Value).ToArray();
            var resp = _dataBase.StringSet(kv);
            if (!resp)
            {
                throw new Exception("batch insertion encoured an error");
            }
        }

        public async Task SetStringAsync(string key, string value, TimeSpan lifeTime)
        {
            await _dataBase.StringSetAsync(key, value, lifeTime);
        }

        public async Task SetStringAsync(string key, string value)
        {
            await _dataBase.StringSetAsync(key, value);
        }

        public async Task SetStringsAsync(Dictionary<string, string> keyValues)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string key, object value, TimeSpan lifeTime)
        {
            var json = _serializer.Serialize(value);
            _dataBase.StringSet(key, json, lifeTime);
        }
        public void SetValue(string key, object value)
        {
            var json = _serializer.Serialize(value);
            _dataBase.StringSet(key, json);
        }

        public async Task SetValueAsync(string key, object value, TimeSpan lifeTime)
        {
            var json = _serializer.Serialize(value);
            await _dataBase.StringSetAsync(key, json, lifeTime);
        }

        public void SetValue(string key, object value, DateTimeOffset expireTime)
        {
            var json = _serializer.Serialize(value);
            _dataBase.StringSet(key, json, expireTime - DateTimeOffset.UtcNow);
        }

        public async Task SetValueAsync(string key, object value, DateTimeOffset expireTime)
        {
            var json = _serializer.Serialize(value);
            await _dataBase.StringSetAsync(key, json, expireTime - DateTimeOffset.UtcNow);
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

        ~RedisCacheService()
        {
            Dispose(false);
        }
        #endregion
    }
}
