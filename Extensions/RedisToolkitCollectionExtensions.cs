using Microsoft.Extensions.DependencyInjection;
using RedisToolkit.Core;
using RedisToolkit.Interfaces.Core;
using RedisToolkit.Interfaces.Datatypes;
using RedisToolkit.Services.Datatypes.RedisSortedSets;

namespace RedisToolkit.Extensions
{
    public static class RedisToolkitCollectionExtensions
    {
        public static IServiceCollection AddRedisToolkit(this IServiceCollection services, string namedConnectionStrings)
        {
            services.AddSingleton<IRedisConnectionBase>(provider =>
            {
                return new RedisConnectionBase(namedConnectionStrings);
            });

            services.AddSingleton<IRedisSortedSetService, RedisSortedSetService>();

            return services;
        }
    }
}
