namespace MUtils.Cache.Redis
{
    public class RedisConfiguration
    {
        public string ConnectionName { get; set; }
        public string Password { get; set; }
        public string[] EndPoints { get; set; }
        public int ConnectTimeout { get; set; } = 100_000;
        public int SyncTimeout { get; set; } = 100_000;
        public bool AbortOnConnectFail { get; set; } = false;

    }
}