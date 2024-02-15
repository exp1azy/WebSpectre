using StackExchange.Redis;

namespace WebSpectre.Server.Services.Interfaces
{
    public interface IMonitoringService
    {
        /// <summary>
        /// Чтение указанного потока Redis.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns></returns>
        public Task SendNetworkFromStreamAsync(string agent, int count, CancellationToken cancellationToken);

        /// <summary>
        /// Чтение потоков Redis.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns></returns>
        public Task SendNetworkFromStreamAsync(int count, CancellationToken cancellationToken);

        /// <summary>
        /// Получить агентов.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns></returns>
        public Task<IEnumerable<RedisKey>> GetAgentsAsync(CancellationToken cancellationToken);
    }
}
