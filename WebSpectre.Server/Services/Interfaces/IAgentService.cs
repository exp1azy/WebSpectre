using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Services.Interfaces
{
    public interface IAgentService
    {
        public Task<string?> GetAgentUrlAsync(string hostname);

        public Task AddAgentAsync(AgentModel agent);

        public Task RemoveAgentAsync(AgentModel agent);
    }
}
