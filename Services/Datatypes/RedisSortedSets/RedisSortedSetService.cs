using RedisToolkit.Interfaces.Core;
using RedisToolkit.Interfaces.Datatypes;
using RedisToolkit.Models;
using RedisToolkit.Utils;
using StackExchange.Redis;

namespace RedisToolkit.Services.Datatypes.RedisSortedSets
{
    public class RedisSortedSetService : IRedisSortedSetService
    {
        private IDatabase client;

        public RedisSortedSetService(IRedisConnectionBase redisConnectionBase)
        {
            this.client = redisConnectionBase.database;
        }

        public async Task<long> GetCountAsync(string sortedSetKey)
        {
            return client.SortedSetLength(sortedSetKey);
        }

        public async Task<long> AddSortedSetsAsync(string sortedSetKey, IEnumerable<Models.SortedSetEntry> entries, int batchSize = 0)
        {
            long totalCount = 0;
            List<StackExchange.Redis.SortedSetEntry> sortedSetEntries = new List<StackExchange.Redis.SortedSetEntry>();

            foreach (var entry in entries)
            {
                sortedSetEntries.Add(new StackExchange.Redis.SortedSetEntry(entry.Member, entry.Score));
            }

            if (batchSize > 0)
            {
                for (int i = 0; i < sortedSetEntries.Count; i += batchSize)
                {
                    var batch = sortedSetEntries.Skip(i).Take(batchSize).ToArray();
                    totalCount += await client.SortedSetAddAsync(sortedSetKey, batch);
                    Console.WriteLine($"Batch from:{i + 1}, Count: {batch.Count()}");
                }
            }
            else
            {
                totalCount += await client.SortedSetAddAsync(sortedSetKey, sortedSetEntries.ToArray());
            }

            return totalCount;
        }

        public async Task<List<SortedSetMember>> GetRangeAsync(string sortedSetKey, long start = 0, long end = -1, Order orderBy = Order.Ascending)
        {
            var memberByRankWithScore = await client.SortedSetRangeByRankWithScoresAsync(sortedSetKey, start, end - 1, orderBy);

            return memberByRankWithScore.Select(x => new SortedSetMember
            {
                Member = x.Element.ToString(),
                Score = x.Score,
                Rank = client.SortedSetRankAsync(sortedSetKey, x.Element, Order.Descending).Result.ToLongOrDefault() + 1,
            }).ToList();
        }

        public async Task<string> GetMemberByRankAsync(string sortedSetKey, long rank, Order orderBy = Order.Ascending)
        {
            var memberByRank = await client.SortedSetRangeByRankWithScoresAsync(sortedSetKey, rank, rank, orderBy, CommandFlags.None);
            return memberByRank.FirstOrDefault().Element;
        }

        public async Task<long> GetRankByMemberAsync(string sortedSetKey, string member, Order orderBy = Order.Ascending)
        {
            var rank = await client.SortedSetRankAsync(sortedSetKey, member, orderBy);
            return rank.ToLongOrDefault();
        }

        public async Task<double> GetScoreByMemberAsync(string sortedSetKey, string member)
        {
            var score = await client.SortedSetScoreAsync(sortedSetKey, member);
            return score.ToDoubleOrDefault();
        }

        public async Task SetExpiry(string key, int expiryInSeconds)
        {
            TimeSpan expirationTime = TimeSpan.FromSeconds(expiryInSeconds);
            await client.KeyExpireAsync(key, expirationTime);
        }

        public async Task<double> IncrementalAddScoreAsync(string sortedSetKey, string member, int score)
        {
            return await client.SortedSetIncrementAsync(sortedSetKey, member, score);
        }

        public async Task<bool> AddScoreAsync(string sortedSetKey, string member, int score)
        {
            return await client.SortedSetAddAsync(sortedSetKey, member, score);

        }
        public async Task<StackExchange.Redis.SortedSetEntry[]> GetSortedSetsListByOrder(string key, int count, Order orderBy = Order.Ascending)
        {
            return await client.SortedSetRangeByRankWithScoresAsync(key, 0, count, orderBy);
        }
    }
    
}
