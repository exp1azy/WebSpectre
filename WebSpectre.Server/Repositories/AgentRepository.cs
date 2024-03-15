using Microsoft.EntityFrameworkCore;
using WebSpectre.Server.Data;
using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Shared.Models;

namespace WebSpectre.Server.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly DataContext _dataContext;

        public AgentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddAgentAsync(AgentModel agent) =>
            await _dataContext.Agents.AddAsync(new Agent { Hostname = agent.HostName, Url = agent.Url });

        public async Task<Agent?> GetAgentAsync(string hostname) =>
            await _dataContext.Agents.FirstOrDefaultAsync(a => a.Hostname == hostname);

        public void RemoveAgent(AgentModel agent) =>
            _dataContext.Agents.Remove(new Agent { Hostname = agent.HostName, Url = agent.Url });
    }
}
