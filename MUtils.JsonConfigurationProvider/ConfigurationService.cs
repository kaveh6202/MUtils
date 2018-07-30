using System;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using MUtils.Configuration;
using MUtils.Interface;
using MUtils.Interface.ConfigurationModel;

namespace MUtils.JsonConfigurationProvider
{
    public class ConfigurationService<T> : IConfigurationService<T> where T : BaseConfigurationModel
    {

        private static string FileName => ConfigurationManager.AppSettings["ConfigFileName"] ?? "Config.json";
        private static string DebugFileName => ConfigurationManager.AppSettings["ConfigFileName-debug"] ?? "Config-debug.json";
        private readonly ISerializer _serializer;
        private readonly ICacher _memoryCache;
        private readonly int _configCacheIntervalInSeconds;
        private const string CacheKey = "configKey";

        public ConfigurationService(ISerializer serializer, ICacher memoryCache)
        {
            _serializer = serializer;
            _memoryCache = memoryCache;
            if (!int.TryParse(ConfigurationManager.AppSettings["ConfigCacheIntervalInSeconds"],
                out _configCacheIntervalInSeconds))
                _configCacheIntervalInSeconds = 300;
        }

        public T Get()
        {
            var key = string.Intern($"{CacheKey}.{typeof(T).Name}");
            var value = _memoryCache.GetValue(key);
            if (value != null)
                return value as T;
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data", FileName);
#if DEBUG
            var debugfilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"App_Data", DebugFileName);
            if (File.Exists(debugfilePath))
                filePath = debugfilePath;
#endif
            var fileContent = File.ReadAllText(filePath);
            var configModel = _serializer.Deserialize<T>(fileContent);
            _memoryCache.SetValue(key, configModel,
                new CacheConfiguration
                {
                    SlidingExpiration = TimeSpan.FromSeconds(_configCacheIntervalInSeconds)
                });
            return configModel;
        }
    }
}