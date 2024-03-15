using WebSpectre.Server.Exceptions;
using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Server.Services.Interfaces;
using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Services
{
    public class AgentService(IAgentRepository agentRepository) : IAgentService
    {
        private readonly IAgentRepository _agentRepository = agentRepository;

        public async Task AddAgentAsync(AgentModel agent)
        {
            var existAgent = await _agentRepository.GetAgentAsync(agent.HostName);
            if (existAgent != null)
                throw new EntityAlreadyExistException();

            await _agentRepository.AddAgentAsync(agent);
        }

        public async Task<string> GetAgentUrlAsync(string hostname)
        {
            var requiredAgent = await _agentRepository.GetAgentAsync(hostname);

            return requiredAgent == null ? throw new NoSuchAgentException() : requiredAgent.Url;
        }

        public async Task RemoveAgentAsync(AgentModel agent)
        {
            _ = await _agentRepository.GetAgentAsync(agent.HostName) ?? throw new NoSuchAgentException();

            _agentRepository.RemoveAgent(agent);
        }
    }
}
