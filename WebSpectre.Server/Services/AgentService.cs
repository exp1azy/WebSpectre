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
        public async Task AddAgentAsync(AgentModel agent, CancellationToken cancellationToken)
        {
            var existAgent = await _agentRepository.GetAgentAsync(agent.Hostname.ToLower());
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
        public async Task<string> GetAgentUrlAsync(string hostname)
        {
            var requiredAgent = await _agentRepository.GetAgentAsync(hostname.ToLower());

            return requiredAgent == null ? throw new NoSuchAgentException() : requiredAgent.Url;
        }

        /// <summary>
        /// Удалить агента из базы данных.
        /// </summary>
        /// <param name="hostname">Имя хоста.</param>
        /// <returns></returns>
        /// <exception cref="NoSuchAgentException"></exception>
        public async Task RemoveAgentAsync(string hostname)
        {
            var agentToDelete = await _agentRepository.GetAgentAsync(hostname) ?? throw new NoSuchAgentException();

            _agentRepository.RemoveAgent(new AgentModel { Hostname = agentToDelete.Hostname, Url = agentToDelete.Url });
        }
    }
}
