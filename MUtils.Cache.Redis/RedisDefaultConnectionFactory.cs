using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sider;
using StackExchange.Redis;

namespace MUtils.Cache.Redis
{
    public class RedisDefaultConnectionFactory : IConnectionFactory
    {
        private static readonly Queue<RedisClient> SimpleClients = new Queue<RedisClient>();
        private static readonly object SimpleClientLock = new object();


        public async Task<ConnectionMultiplexer> GetConnection(RedisConfiguration configuration)
        {
            ConfigurationOptions options = GetRedisOptions(configuration);
            return await ConnectionMultiplexer.ConnectAsync(options);
        }

        public RedisClient GetSimpleConnection(RedisConfiguration configuration)
        {
            if (!SimpleClients.Any())
                lock (SimpleClientLock)
                    if (!SimpleClients.Any())
                    {
                        foreach (var configurationEndPoint in configuration.EndPoints)
                        {
                            var redisSettings = RedisSettings.Build();
                            if (!string.IsNullOrEmpty(configuration.Password)) redisSettings.Auth(configuration.Password);
                            redisSettings.Host(configurationEndPoint.Split(':')[0]);
                            redisSettings.ConnectionTimeout(configuration.ConnectTimeout);
                            redisSettings.Port(int.Parse(configurationEndPoint.Split(':')[1]));
                            var rc = new RedisClient(redisSettings);
                            SimpleClients.Enqueue(rc);
                        }
                    }
            lock (SimpleClientLock)
            {
                var item = SimpleClients.Dequeue();
                SimpleClients.Enqueue(item);
                return item;
            }
        }

        public async Task<IDatabase> GetDataBase(RedisConfiguration configuration, int db = 0)
        {
            ConnectionMultiplexer connection = await GetConnection(configuration);
            return connection.GetDatabase(db);
        }

        #region Private Methods
        private ConfigurationOptions GetRedisOptions(RedisConfiguration config)
        {
            ConfigurationOptions options = new ConfigurationOptions()
            {
                AbortOnConnectFail = config.AbortOnConnectFail,
                ClientName = config.ConnectionName,
                ConnectTimeout = config.ConnectTimeout,
                SyncTimeout = config.SyncTimeout,
                Password = config.Password
            };

            for (int i = 0; i < config.EndPoints.Length; i++)
                options.EndPoints.Add(config.EndPoints[i]);
            return options;
        }
        #endregion
    }
}
