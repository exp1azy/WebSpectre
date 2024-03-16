using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using PacketDotNet;
using System.Net.Http.Json;
using WebSpectre.Client.EventArgs;
using WebSpectre.Shared;
using WebSpectre.Shared.Agents;
using WebSpectre.Shared.Perfomance;
using ErrorEventArgs = WebSpectre.Client.EventArgs.ErrorEventArgs;
using StatisticsEventArgs = WebSpectre.Client.EventArgs.StatisticsEventArgs;

namespace WebSpectre.Client.Services
{
    public class NetworkHandler
    {
        private readonly HttpClient _httpClient;
        private readonly HubConnection _hub;

        private Dictionary<string, string?> _hosts;

        public NetworkHandler(HttpClient httpClient, HubConnection hub) 
        {
            _httpClient = httpClient;
            _hub = hub;

            HubOn();
            _ = GetAgentsAsync();
        }

        public async Task<bool> GetHostsInfoAsync()
        {
            if (_hosts == null)
                return false;

            var hosts = new Dictionary<string, HostInfo?>();

            foreach (var host in _hosts)
            {
                HostInfo? hostInfo = null;

                if (host.Value != null)
                {
                    try
                    {
                        hostInfo = await _httpClient.GetFromJsonAsync<HostInfo>($"{host.Value}/pcap/info");
                    }
                    catch { }
                }
                   
                hosts.Add(host.Key, hostInfo);
            }

            OnHostsReceived.Invoke(this, new HostsEventArgs { Hosts = hosts });

            return true;          
        }

        public async Task<bool> GetAgentsStatusAsync()
        {
            if (_hosts == null)
                return false;

            var hostsStatus = new Dictionary<string, bool?>();

            foreach (var agent in _hosts)
            {
                bool? status = null;

                if (agent.Value != null)
                {
                    try
                    {
                        var response = await _httpClient.GetAsync($"{agent.Value}/pcap/status");
                        if (response.IsSuccessStatusCode)
                        {
                            status = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);
                        }
                    }
                    catch { }
                }

                hostsStatus.Add(agent.Key, status);
            }

            OnStatusReceived.Invoke(this, new AgentsStatusEventArgs { AgentsStatus = hostsStatus });

            return true;
        }

        public async Task StartSniffForRequired(string host, string adapter)
        {
            var url = _hosts[host];

            try
            {
                var response = await _httpClient.GetAsync($"{url}/pcap/start?a={adapter}");
            }
            catch
            {

            }
        }

        public async Task StopSniffForRequired(string host)
        {
            var url = _hosts[host];

            try
            {
                var response = await _httpClient.GetAsync($"{url}/pcap/stop");
            }
            catch
            {

            }
        }

        public async Task GetHostnamesAsync()
        {
            if (_hub is not null)
                await _hub.SendAsync("GetHostnames");
        }

        public async Task StartMonitorForRequired(string host)
        {
            if (_hub is not null)           
                await _hub.SendAsync("StartRequired", host, null);           
        }

        public async Task StopMonitorForRequired(string host)
        {
            if (_hub is not null)           
                await _hub.SendAsync("StopRequired", host);
        }

        private async Task GetAgentsAsync()
        {
            if (_hub is not null)
                await _hub.SendAsync("GetAgents");
        }

        private void HubOn()
        {
            _hub.On<string>("ReceiveError", (error) =>
            {
                OnErrorReceived.Invoke(this, new ErrorEventArgs { Error = error });
            });

            _hub.On<List<string>>("ReceiveHostnames", (hostnames) =>
            {
                OnHostnamesReceived.Invoke(this, new HostnamesEventArgs { Hostnames = hostnames });
            });

            _hub.On<Dictionary<string, string?>>("ReceiveAgents", (agents) =>
            {
                _hosts = agents;
                _ = GetHostsInfoAsync();
            });

            _hub.On<string>("ReceiveMessage", (message) =>
            {
                OnMessageReceived.Invoke(this, new MessageEventArgs { Message = message });
            });

            _hub.On<ulong>("ReceiveDelay", (delay) =>
            {
                OnDelayReceived.Invoke(this, new DelayEventArgs { Delay = delay });
            });

            _hub.On<Jitter>("ReceiveJitter", (jitter) =>
            {
                OnJitterReceived.Invoke(this, new JitterEventArgs { Jitter = jitter });
            });

            _hub.On<Packet>("ReceivePacket", (packet) =>
            {
                OnPacketReceived.Invoke(this, new PacketEventArgs { Packet = packet });
            });

            _hub.On<List<Statistics>>("ReceiveStatistics", (statistics) =>
            {
                OnStatisticsReceived.Invoke(this, new StatisticsEventArgs { Statistics = statistics });
            });

            _hub.On<Throughput>("ReceiveThroughput", (throughput) =>
            {
                OnThroughputReceived.Invoke(this, new ThroughputEventArgs { Throughput = throughput });
            });

            _hub.StartAsync();
        }

        public event EventHandler<ErrorEventArgs> OnErrorReceived;
        public event EventHandler<HostnamesEventArgs> OnHostnamesReceived;
        public event EventHandler<HostsEventArgs> OnHostsReceived;
        public event EventHandler<AgentsStatusEventArgs> OnStatusReceived;
        public event EventHandler<MessageEventArgs> OnMessageReceived;
        public event EventHandler<DelayEventArgs> OnDelayReceived;
        public event EventHandler<JitterEventArgs> OnJitterReceived;
        public event EventHandler<PacketEventArgs> OnPacketReceived;
        public event EventHandler<StatisticsEventArgs> OnStatisticsReceived;
        public event EventHandler<ThroughputEventArgs> OnThroughputReceived;
    }
}
