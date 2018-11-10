using System.Threading.Tasks;
using StackExchange.Redis;

namespace MUtils.Cache.Redis
{
    public class RedisDefaultConnectionFactory : IConnectionFactory
    {
        public async Task<ConnectionMultiplexer> GetConnection(RedisConfiguration configuration)
        {
            ConfigurationOptions options = GetRedisOptions(configuration);
            return await ConnectionMultiplexer.ConnectAsync(options);
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
