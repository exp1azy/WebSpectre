using Microsoft.EntityFrameworkCore;
using WebSpectre.Server.Data;
using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Repositories
{
    /// <summary>
    /// Класс, позволяющий выполнять операции с агентами в базе данных.
    /// </summary>
    public class AgentRepository(DataContext dataContext) : IAgentRepository
    {
        private readonly DataContext _dataContext = dataContext;

        /// <summary>
        /// Добавить агента.
        /// </summary>
        /// <param name="agent">Агент.</param>
        /// <returns></returns>
        public async Task AddAgentAsync(AgentModel agent, CancellationToken cancellationToken) =>
            await _dataContext.Agents.AddAsync(new Agent { Hostname = agent.Hostname, Url = agent.Url }, cancellationToken);

        /// <summary>
        /// Получить агента по имени хоста.
        /// </summary>
        /// <param name="hostname">Имя хоста.</param>
        /// <returns>Если агент существует, возвращает <see cref="Agent"/>, иначе <see cref="null"/></returns>
        public async Task<Agent?> GetAgentAsync(string hostname) =>
            await _dataContext.Agents.FirstOrDefaultAsync(a => a.Hostname == hostname);

        /// <summary>
        /// Удалить агента.
        /// </summary>
        /// <param name="agent">Агент.</param>
        public void RemoveAgent(AgentModel agent) =>
            _dataContext.Agents.Remove(new Agent { Hostname = agent.Hostname, Url = agent.Url });
    }
}
