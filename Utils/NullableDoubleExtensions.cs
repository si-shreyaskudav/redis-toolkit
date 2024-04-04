using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisToolkit.Utils
{
    public static class NullableDoubleExtensions
    {
        public static double ToDoubleOrDefault(this double? nullableDouble, double defaultValue = 0)
        {
            return nullableDouble.HasValue ? nullableDouble.Value : defaultValue;
        }
    }
}
