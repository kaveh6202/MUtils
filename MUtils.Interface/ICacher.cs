using System.Collections.Generic;
using MUtils.Interface.ConfigurationModel;

namespace MUtils.Interface
{
    public interface ICacher
    {
        object GetValue(string key);
        IDictionary<string, object> GetValues(IList<string> keys);
        void SetValue(string key, object value, CacheConfiguration policy);
        void SetValues(Dictionary<string, object> keyValues, CacheConfiguration policy);
        void Delete(string key);
        void Delete(IEnumerable<string> keys);
    }
}