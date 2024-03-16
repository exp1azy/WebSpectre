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
        public async Task AddAgentAsync(AgentModel agent)
        {
            var existAgent = await _agentRepository.GetAgentAsync(agent.HostName);
            if (existAgent != null)
                throw new EntityAlreadyExistException();

            await _agentRepository.AddAgentAsync(agent);
        }

        /// <summary>
        /// Получить URL указанного агента из базы данных.
        /// </summary>
        /// <param name="hostname">Имя хоста.</param>
        /// <returns>Строковое представление URL.</returns>
        /// <exception cref="NoSuchAgentException"></exception>
        public async Task<string> GetAgentUrlAsync(string hostname)
        {
            var hostnameLower = hostname.ToLower();
            var requiredAgent = await _agentRepository.GetAgentAsync(hostnameLower);

            return requiredAgent == null ? throw new NoSuchAgentException() : requiredAgent.Url;
        }

        /// <summary>
        /// Удалить агента из базы данных.
        /// </summary>
        /// <param name="agent">Агент.</param>
        /// <returns></returns>
        /// <exception cref="NoSuchAgentException"></exception>
        public async Task RemoveAgentAsync(AgentModel agent)
        {
            _ = await _agentRepository.GetAgentAsync(agent.HostName) ?? throw new NoSuchAgentException();

            _agentRepository.RemoveAgent(agent);
        }
    }
}
