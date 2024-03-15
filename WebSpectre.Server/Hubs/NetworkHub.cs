using Microsoft.AspNetCore.SignalR;
using WebSpectre.Server.Services.Interfaces;

namespace WebSpectre.Server.Hubs
{
    public class NetworkHub(IMonitoringService monitoringService) : Hub
    {
        private readonly IMonitoringService _monitoringService = monitoringService;

        public async Task GetAgents() =>      
            await _monitoringService.SendAgentsAsync();
        
        public async Task StartRequired(string agent, int? count = null) =>       
            await _monitoringService.SendNetworkFromRequiredAgentAsync(agent, count);       

        public async Task StopRequired(string agent) =>
            await _monitoringService.StopRequiredAsync(agent);
    }
}
