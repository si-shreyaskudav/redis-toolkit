using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToolkit.Utils
{
    public static class NullableLongExtensions
    {
        public static long ToLongOrDefault(this long? nullableLong, long defaultValue = 0)
        {
            return nullableLong.HasValue ? nullableLong.Value : defaultValue;
        }
    }
}
