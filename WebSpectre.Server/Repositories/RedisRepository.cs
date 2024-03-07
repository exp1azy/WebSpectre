using StackExchange.Redis;
using WebSpectre.Server.Exceptions;
using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Server.Resources;

namespace WebSpectre.Server.Repositories
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDatabase _redisDatabase;
        private readonly IConnectionMultiplexer? _redisConnection;

        public RedisRepository(IConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            _redisDatabase = _redisConnection.GetDatabase();
        }

        public IEnumerable<RedisKey> GetRedisKeys(string host, int port)
        {
            IServer? server = default;

            try
            {
                server = _redisConnection!.GetServer(host, port);
            }
            catch (RedisConnectionException ex)
            {
                throw new WebSpectreRedisConnectionException(Error.NoConnectionToRedis, ex);
            }

            foreach (var key in server!.Keys(pattern: "host_*"))
                yield return new RedisKey(key);
        }

        public async Task<StreamEntry[]> ReadStreamAsync(RedisKey key, RedisValue offset, int? count = null) =>
            await _redisDatabase.StreamReadAsync(key, offset, count);
    }
}
