using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MUtils.Cache.Redis
{
    public interface IRedisCacheService
    {
        void Delete(string key);
        Task DeleteAsync(string key);
        void Dispose();
        string GetString(string key, string region = null);
        Task<string> GetStringAsync(string key, string region = null);
        string[] GetStrings(string[] keys, string region = null);
        Task<string[]> GetStringsAsync(string[] keys, string region = null);
        T GetValue<T>(string key, string region = null);
        Task<T> GetValueAsync<T>(string key, string region = null);
        IDictionary<string, T> GetValues<T>(string[] keys);
        Task<IDictionary<string, T>> GetValuesAsync<T>(string[] keys);
        void SetString(string key, string value, TimeSpan lifeTime);
        Task SetStringAsync(string key, string value, TimeSpan lifeTime);
        void SetValue(string key, object value, DateTimeOffset expireTime);
        void SetValue(string key, object value, TimeSpan lifeTime);
        Task SetValueAsync(string key, object value, DateTimeOffset expireTime);
        Task SetValueAsync(string key, object value, TimeSpan lifeTime);
        void SetString(string key, string value);
        void SetStrings(Dictionary<string, string> keyValues);
        Task SetStringAsync(string key, string value);
        Task SetStringsAsync(Dictionary<string, string> keyValues);
        void SetValue(string key, object value);
    }
}