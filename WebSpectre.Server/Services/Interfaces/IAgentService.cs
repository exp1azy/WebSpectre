using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Services.Interfaces
{
    /// <summary>
    /// Интерфейс, выполняющий операции с агентами.
    /// </summary>
    public interface IAgentService
    {
        /// <summary>
        /// Получить URL указанного агента из базы данных.
        /// </summary>
        /// <param name="hostname">Имя хоста.</param>
        /// <returns>Строковое представление URL.</returns>
        public Task<string?> GetAgentUrlAsync(string hostname);

        /// <summary>
        /// Добавить агента в базу данных.
        /// </summary>
        /// <param name="agent">Агент.</param>
        /// <returns></returns>
        public Task AddAgentAsync(AgentModel agent, CancellationToken cancellationToken);

        /// <summary>
        /// Удалить агента из базы данных.
        /// </summary>
        /// <param name="hostname">Имя хоста.</param>
        /// <returns></returns>
        public Task RemoveAgentAsync(string hostname);
    }
}
