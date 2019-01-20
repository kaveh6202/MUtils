using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
//using CSRedis;
using StackExchange.Redis;

namespace MUtils.Cache.Redis
{
    public class RedisDefaultConnectionFactory : IConnectionFactory
    {
        private static readonly Queue<string> SimpleClients = new Queue<string>();
        //private static RedisClient _currentClient = null;
        private static readonly object SimpleClientLock = new object();


        public async Task<ConnectionMultiplexer> GetConnection(RedisConfiguration configuration)
        {
            ConfigurationOptions options = GetRedisOptions(configuration);
            return await ConnectionMultiplexer.ConnectAsync(options);
        }

        public ConnectionMultiplexer GetConnectionSync(RedisConfiguration configuration)
        {
            ConfigurationOptions options = GetRedisOptions(configuration);
            return ConnectionMultiplexer.Connect(options);
        }

        //public RedisClient GetSimpleConnection(RedisConfiguration configuration)
        //{
        //    if (_currentClient != null) return _currentClient;
        //    if (!SimpleClients.Any())
        //        lock (SimpleClientLock)
        //            if (!SimpleClients.Any())
        //            {
        //                foreach (var configurationEndPoint in configuration.EndPoints)
        //                {
        //                    SimpleClients.Enqueue(configurationEndPoint);
        //                }
        //            }
        //    lock (SimpleClientLock)
        //    {
        //        var item = SimpleClients.Dequeue();
        //        var rc = new RedisClient(item.Split(':')[0],
        //            int.Parse(item.Split(':')[1]));
        //        _currentClient = rc;
        //        SimpleClients.Enqueue(item);
        //        return _currentClient;
        //    }
        //}

        //public RedisClient NewSimpleConnection()
        //{
        //    lock (SimpleClientLock)
        //    {
        //        var item = SimpleClients.Dequeue();
        //        var rc = new RedisClient(item.Split(':')[0],
        //            int.Parse(item.Split(':')[1]));
        //        _currentClient = rc;
        //        SimpleClients.Enqueue(item);
        //        return _currentClient;
        //    }
        //}

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
