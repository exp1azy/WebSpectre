using WebSpectre.Server.Data;
using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Repositories.Interfaces
{
    public interface IAgentRepository
    {
        public Task AddAgentAsync(AgentModel agent);

        public Task<Agent?> GetAgentAsync(string hostname);

        public void RemoveAgent(AgentModel agent);
    }
}
