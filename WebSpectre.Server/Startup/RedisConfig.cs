using StackExchange.Redis;
using WebSpectre.Server.Resources;

namespace WebSpectre.Server.Startup
{
    public static class RedisConfig
    {
        public static void AddRedis(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var connection = builder.Configuration.GetConnectionString("RedisConnection");
                if (string.IsNullOrEmpty(connection))
                    throw new ArgumentNullException(Error.FailedToReadRedisConnectionString);

                return ConnectionMultiplexer.Connect(connection);
            });
        }
    }
}
