using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Services.Interfaces
{
    /// <summary>
    /// Интерфейс, выполняющий операции с агентами.
    /// </summary>
    public interface IAgentService
    {
        /// <summary>
        /// Получить всех агентов из базы данных.
        /// </summary>
        /// <returns>Список агентов.</returns>
        public Task<List<AgentModel>> GetAllAgentsAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Получить URL указанного агента из базы данных.
        /// </summary>
        /// <param name="hostname">Имя хоста.</param>
        /// <returns>Строковое представление URL.</returns>
        public Task<string?> GetAgentUrlAsync(string hostname, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавить агента в базу данных.
        /// </summary>
        /// <param name="agent">Агент.</param>
        /// <returns></returns>
        public Task AddAgentAsync(AgentModel agent, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удалить агента из базы данных.
        /// </summary>
        /// <param name="hostname">Имя хоста.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns></returns>
        public Task RemoveAgentAsync(string hostname, CancellationToken cancellationToken = default);
    }
}
