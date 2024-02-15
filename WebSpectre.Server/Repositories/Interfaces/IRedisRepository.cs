using StackExchange.Redis;

namespace WebSpectre.Server.Repositories.Interfaces
{
    public interface IRedisRepository
    {
        /// <summary>
        /// Метод, возвращающий агентов из сервера Redis.
        /// </summary>
        /// <returns>Список ключей.</returns>
        public IEnumerable<RedisKey> GetRedisKeys(string host, int port);

        /// <summary>
        /// Чтение потока Redis.
        /// </summary>
        /// <param name="key">Ключ потока.</param>
        /// <param name="offset">Смещение.</param>
        /// <param name="count">Количество элементов в потоке.</param>
        /// <returns>Массив <see cref="StreamEntry"/>, представляющий элементы потока.</returns>
        public Task<StreamEntry[]> ReadStreamAsync(RedisKey key, RedisValue offset, int count);
    }
}
