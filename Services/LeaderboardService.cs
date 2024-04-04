using RedisToolkit.Interfaces;
using RedisToolkit.Interfaces.Core;
using RedisToolkit.Interfaces.Datatypes;
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
        private IDatabase client;
        private readonly IRedisSortedSetService redisSortedSetService;

        public LeaderboardService(IRedisConnectionBase redisConnectionBase, IRedisSortedSetService redisSortedSetService)
        {
            this.client = redisConnectionBase.database;
            this.redisSortedSetService = redisSortedSetService;
        }

        public async Task<SortedSetMember> GetMemberRank(string sortedSetKey, string member)
        {
            var memberScore = await client.SortedSetScoreAsync(sortedSetKey, member);

            // If user does not exists in RSS, SortedSetRankAsync returns 0, so handled to force return null
            if (memberScore == null)
                return null;

            var _memberScore = memberScore.ToDoubleOrDefault();

            var membersAtRank = await client.SortedSetRangeByScoreAsync(sortedSetKey, _memberScore, _memberScore, Exclude.None, Order.Descending, 0, 1);
            var firstMember = membersAtRank[0];

            var userRank = await client.SortedSetRankAsync(sortedSetKey, firstMember, Order.Descending);

            SortedSetMember currentPlayer = new SortedSetMember
            {
                Member = member,
                Rank = userRank.ToLongOrDefault(),
                Score = _memberScore,
            };

            return currentPlayer;
        }
    }
}
