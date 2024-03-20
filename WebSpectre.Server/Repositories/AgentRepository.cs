using Microsoft.EntityFrameworkCore;
using Nest;
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
        /// Получить агента по имени хоста.
        /// </summary>
        /// <param name="hostname">Имя хоста.</param>
        /// <returns>Если агент существует, возвращает <see cref="Agent"/>, иначе <see cref="null"/></returns>
        public async Task<Agent?> GetAgentAsync(string hostname, CancellationToken cancellationToken = default) =>
            await _dataContext.Agents.FirstOrDefaultAsync(a => a.Hostname == hostname, cancellationToken);

        /// <summary>
        /// Получить всех агентов.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns>Список агентов.</returns>
        public async Task<List<Agent>> GetAllAsync(CancellationToken cancellationToken = default) =>        
            await _dataContext.Agents.ToListAsync(cancellationToken);
        
        /// <summary>
        /// Добавить агента.
        /// </summary>
        /// <param name="agent">Агент.</param>
        /// <returns></returns>
        public async Task AddAgentAsync(AgentModel agent, CancellationToken cancellationToken = default)
        {
            await _dataContext.Agents.AddAsync(new Agent { Hostname = agent.Hostname, Url = agent.Url }, cancellationToken);

            await _dataContext.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удалить агента.
        /// </summary>
        /// <param name="agentToDelete">Агент.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns></returns>
        public async Task RemoveAgentAsync(Agent agentToDelete, CancellationToken cancellationToken = default)
        {
            _dataContext.Agents.Remove(agentToDelete);
            await _dataContext.SaveChangesAsync(cancellationToken);          
        }
    }
}
