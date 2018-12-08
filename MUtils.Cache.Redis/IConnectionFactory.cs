using System.Threading.Tasks;
using Sider;
using StackExchange.Redis;

namespace MUtils.Cache.Redis
{
    public interface IConnectionFactory
    {
        Task<IDatabase> GetDataBase(RedisConfiguration configuration, int db);
        Task<ConnectionMultiplexer> GetConnection(RedisConfiguration configuration);
        RedisClient GetSimpleConnection(RedisConfiguration configuration);
    }
}