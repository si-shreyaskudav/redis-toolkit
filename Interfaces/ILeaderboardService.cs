using RedisToolkit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToolkit.Interfaces
{
    public interface ILeaderboardService
    {
        Task<SortedSetMember> GetMemberRank(string sortedSetKey, string member);

    }
}
