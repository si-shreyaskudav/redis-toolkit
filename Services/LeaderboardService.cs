using RedisToolkit.Interfaces;
using RedisToolkit.Interfaces.Core;
using RedisToolkit.Interfaces.Datatypes;
using RedisToolkit.Interfaces.LuaScript;
using RedisToolkit.Models;
using RedisToolkit.Services.Datatypes.RedisSortedSets;
using RedisToolkit.Utils;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToolkit.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private IDatabase _client;
        private readonly IRedisSortedSetService _redisSortedSetService;
        private readonly ILuaService _luaService;

        public LeaderboardService(IRedisBase redisBase, IRedisSortedSetService redisSortedSetService,
            ILuaService luaService)
        {
            this._client = redisBase.client;
            this._redisSortedSetService = redisSortedSetService;
            this._luaService = luaService;
        }

        public async Task<SortedSetMember> GetLeaderboardMember(string sortedSetKey, string member)
        {
            var memberScore = await _client.SortedSetScoreAsync(sortedSetKey, member);

            // If user does not exists in RSS, SortedSetRankAsync returns 0, so handled to force return null
            if (memberScore == null)
                return null;

            var _memberScore = memberScore.ToDoubleOrDefault();

            var membersAtRank = await _client.SortedSetRangeByScoreAsync(sortedSetKey, _memberScore, _memberScore, Exclude.None, Order.Descending, 0, 1);
            var firstMember = membersAtRank[0];

            var userRank = await _client.SortedSetRankAsync(sortedSetKey, firstMember, Order.Descending);

            SortedSetMember currentPlayer = new SortedSetMember
            {
                Member = member,
                Rank = userRank.ToLongOrDefault(),
                Score = _memberScore,
            };

            return currentPlayer;
        }

        public async Task<IEnumerable<SortedSetMember>> GetLeaderboardMembers(string sortedSetKey, long count)
        {
            var members = await _redisSortedSetService.GetRangeAsync(sortedSetKey, 0, count, Order.Descending, false);

            string luaScript = await File.ReadAllTextAsync("ranks-by-scores");
            var scores = members.Select(x => (RedisValue)x.Score).Distinct().ToArray();
            RedisKey[] keys = { sortedSetKey };
            RedisValue[] values = scores.ToArray();

            var scoreRanks = await _luaService.ExecuteLuaScriptAsync(luaScript, keys, values);
            Dictionary<double, long> scoreRankMap = ConvertRedisValueToDictionary(scoreRanks);   // <score, rank> mapping

            return members.Select(member =>
            {
                return new SortedSetMember
                {
                    Member = member.Member,
                    Score = member.Score,
                    Rank = scoreRankMap[member.Score]
                };
            }).ToList();
        }

        private Dictionary<double, long> ConvertRedisValueToDictionary(RedisResult[] scoreRanks)
        {
            Dictionary<double, long> scoreRankMap = new Dictionary<double, long>();

            for (int i = 0; i < scoreRanks.Length; i++)
            {
                RedisValue[] scoreAndRank = (RedisValue[])scoreRanks[i];

                double score = (double)scoreAndRank[0];
                long rank = (long)scoreAndRank[1];

                scoreRankMap[score] = rank;
            }

            return scoreRankMap;
        }
    }
}
