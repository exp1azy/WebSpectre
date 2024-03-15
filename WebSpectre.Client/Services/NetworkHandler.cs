using Microsoft.AspNetCore.SignalR.Client;
using PacketDotNet;
using System.Net.Http.Json;
using WebSpectre.Shared;
using WebSpectre.Shared.Agents;
using WebSpectre.Shared.Perfomance;

namespace WebSpectre.Client.Services
{
    public class NetworkHandler
    {
        private readonly HttpClient _httpClient;
        private readonly HubConnection _hub;

        private string _error;
        private Dictionary<string, string?> _agents;
        private Dictionary<string, HostInfo> _hosts;

        public NetworkHandler(HttpClient httpClient, HubConnection hub) 
        {
            _httpClient = httpClient;
            _hub = hub;

            _agents = new Dictionary<string, string>();
            _hosts = new Dictionary<string, HostInfo>();

            HubOn();
        }

        public Dictionary<string, string> Agents => _agents;

        public Dictionary<string, HostInfo> Hosts => _hosts;

        public string Error => _error;

        public async Task GetHostsInfo()
        {
            if (_agents != null)
            {
                foreach (var agent in _agents)
                { 
                    var hostInfo = await _httpClient.GetFromJsonAsync<HostInfo>(agent.Value);

                    if (hostInfo != null)
                        _hosts.Add(agent.Key, hostInfo);
                }
            }
        }

        public async Task GetAgentsAsync()
        {
            if (_hub is not null)          
                await _hub.SendAsync("GetAgents");       
        }

        public async Task StartRequired(string agent)
        {
            if (_hub is not null)           
                await _hub.SendAsync("StartRequired", agent, null);           
        }

        public async Task StopRequired(string agent)
        {
            if (_hub is not null)           
                await _hub.SendAsync("StopRequired", agent);          
        }

        private void HubOn()
        {
            _hub.On<string>("ReceiveError", (error) =>
            {
                _error = error;
            });

            _hub.On<Dictionary<string, string?>>("ReceiveAgents", (agents) =>
            {
                _agents = agents;
            });

            _hub.On<string>("ReceiveMessage", (message) =>
            {
                
            });

            _hub.On<ulong>("ReceiveDelay", (delay) =>
            {
                
            });

            _hub.On<Jitter>("ReceiveJitter", (jitter) =>
            {
                
            });

            _hub.On<Packet>("ReceivePacket", (packet) =>
            {
                
            });

            _hub.On<List<Statistics>>("ReceiveStatistics", (statistics) =>
            {
                
            });

            _hub.On<Throughput>("ReceiveThroughput", (throughput) =>
            {
                
            });

            _hub.StartAsync();
        }
    }
}
