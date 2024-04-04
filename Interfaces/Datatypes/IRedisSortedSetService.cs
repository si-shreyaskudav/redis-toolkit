using RedisToolkit.Models;
using StackExchange.Redis;

namespace RedisToolkit.Interfaces.Datatypes
{
    public interface IRedisSortedSetService
    {
        Task<long> GetRankByMemberAsync(string sortedSetKey, string member, Order orderBy = Order.Ascending);
        Task<string> GetMemberByRankAsync(string sortedSetKey, long rank, Order orderBy = Order.Ascending);
        Task<long> AddSortedSetsAsync(string sortedSetKey, IEnumerable<Models.SortedSetEntry> entries, int batchSize = -1);
        Task SetExpiry(string key, int expiryInSeconds);
        Task<double> IncrementalAddScoreAsync(string sortedSetKey, string member, int score);
        Task<StackExchange.Redis.SortedSetEntry[]> GetSortedSetsListByOrder(string key, int count, Order orderBy = Order.Ascending);
        Task<long> GetCountAsync(string sortedSetKey);
        Task<List<SortedSetMember>> GetRangeAsync(string sortedSetKey, long start = 0, long end = -1, Order orderBy = Order.Ascending);
        Task<bool> AddScoreAsync(string sortedSetKey, string member, int score);
        Task<double> GetScoreByMemberAsync(string sortedSetKey, string member);
    }
}
