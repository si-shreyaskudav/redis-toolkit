using RedisToolkit.Interfaces.Core;
using StackExchange.Redis;

namespace RedisToolkit.Core
{
    public class RedisBase : IRedisBase
    {
        public IDatabase client { get; set; }

        public RedisBase(string connectionString)
        {
            var redis = ConnectionMultiplexer.Connect(connectionString);
            client = redis.GetDatabase();
        }

    }
}
