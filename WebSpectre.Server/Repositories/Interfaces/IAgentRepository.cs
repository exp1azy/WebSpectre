using WebSpectre.Server.Data;
using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Repositories.Interfaces
{
    /// <summary>
    /// Интерфейс, позволяющий выполнять операции с агентами в базе данных.
    /// </summary>
    public interface IAgentRepository
    {
        /// <summary>
        /// Добавить агента.
        /// </summary>
        /// <param name="agent">Агент.</param>
        /// <returns></returns>
        public Task AddAgentAsync(AgentModel agent);

        /// <summary>
        /// Получить агента по имени хоста.
        /// </summary>
        /// <param name="hostname">Имя хоста.</param>
        /// <returns>Если агент существует, возвращает <see cref="Agent"/>, иначе <see cref="null"/></returns>
        public Task<Agent?> GetAgentAsync(string hostname);

        /// <summary>
        /// Удалить агента.
        /// </summary>
        /// <param name="agent">Агент.</param>
        public void RemoveAgent(AgentModel agent);
    }
}
