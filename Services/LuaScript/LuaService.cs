using RedisToolkit.Interfaces.Core;
using RedisToolkit.Interfaces.LuaScript;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToolkit.Services.LuaScript
{
    public class LuaService : ILuaService
    {
        private IDatabase client;

        public LuaService(IRedisBase redisConnectionBase)
        {
            this.client = redisConnectionBase.client;
        }
        public async Task<RedisResult[]> ExecuteLuaScriptAsync(string luaScript, RedisKey[] keys, RedisValue[] values)
        {
            return (RedisResult[])await client.ScriptEvaluateAsync(luaScript, keys, values);
        }

    }
}
