using WebSpectre.Server.Exceptions;
using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Server.Services.Interfaces;
using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Services
{
    /// <summary>
    /// Класс, выполняющий операции с агентами.
    /// </summary>
    public class AgentService(IAgentRepository agentRepository) : IAgentService
    {
        private readonly IAgentRepository _agentRepository = agentRepository;

        /// <summary>
        /// Добавить агента в базу данных.
        /// </summary>
        /// <param name="agent">Агент.</param>
        /// <returns></returns>
        /// <exception cref="EntityAlreadyExistException"></exception>
        public async Task AddAgentAsync(AgentModel agent, CancellationToken cancellationToken = default)
        {
            var existAgent = await _agentRepository.GetAgentAsync(agent.Hostname.ToLower(), cancellationToken);
            if (existAgent != null)
                throw new EntityAlreadyExistException();

            await _agentRepository.AddAgentAsync(agent, cancellationToken);
        }

        /// <summary>
        /// Получить URL указанного агента из базы данных.
        /// </summary>
        /// <param name="hostname">Имя хоста.</param>
        /// <returns>Строковое представление URL.</returns>
        /// <exception cref="NoSuchAgentException"></exception>
        public async Task<string> GetAgentUrlAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var requiredAgent = await _agentRepository.GetAgentAsync(hostname.ToLower(), cancellationToken);

            return requiredAgent == null ? throw new NoSuchAgentException() : requiredAgent.Url;
        }

        /// <summary>
        /// Получить всех агентов из базы данных.
        /// </summary>
        /// <returns>Список агентов.</returns>
        public async Task<List<AgentModel>> GetAllAgentsAsync(CancellationToken cancellationToken = default)
        {
            var agents = await _agentRepository.GetAllAsync(cancellationToken);
            var agentsToReturn = new List<AgentModel>();

            foreach (var agent in agents)
            {
                agentsToReturn.Add(new AgentModel { Hostname = agent.Hostname, Url = agent.Url });
            }

            return agentsToReturn;
        }

        /// <summary>
        /// Удалить агента из базы данных.
        /// </summary>
        /// <param name="hostname">Имя хоста.</param>
        /// <param name="cancellationToken">Токен отмены.</param>
        /// <returns></returns>
        /// <exception cref="NoSuchAgentException"></exception>
        public async Task RemoveAgentAsync(string hostname, CancellationToken cancellationToken = default)
        {
            var agentToDelete = await _agentRepository.GetAgentAsync(hostname, cancellationToken) ?? throw new NoSuchAgentException();

            await _agentRepository.RemoveAgentAsync(agentToDelete, cancellationToken);
        }
    }
}
