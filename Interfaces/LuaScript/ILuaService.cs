using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToolkit.Interfaces.LuaScript
{
    public interface ILuaService
    {
        Task<RedisResult[]> ExecuteLuaScriptAsync(string luaScript, RedisKey[] keys, RedisValue[] values);
    }
}
