using RedisToolkit.Interfaces.Core;
using StackExchange.Redis;

namespace RedisToolkit.Core
{
    public class RedisConnectionBase : IRedisConnectionBase
    {
        public IDatabase database { get; set; }

        public RedisConnectionBase(string connectionString)
        {
            var redis = ConnectionMultiplexer.Connect(connectionString);
            database = redis.GetDatabase();
        }

    }
}
