
namespace RedisToolkit.Models
{
    public class SortedSetMember
    {
        public string Member { get; set; }
        public long Rank { get; set; }
        public double Score { get; set; }
    }
}
