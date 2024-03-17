using Microsoft.AspNetCore.SignalR;
using PacketDotNet;
using Serilog;
using StackExchange.Redis;
using WebSpectre.Shared.Perfomance;
using WebSpectre.Server.Exceptions;
using WebSpectre.Server.Hubs;
using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Server.Services.Interfaces;
using WebSpectre.Shared.Services;
using Error = WebSpectre.Server.Resources.Error;

namespace WebSpectre.Server.Services
{
    public class MonitoringService : IMonitoringService
    {
        private readonly PerfomanceCalculator _perfomanceCalc;
        private readonly IRedisRepository _redisRepository;
        private readonly IHubContext<NetworkHub> _hubContext;
        private readonly IConfiguration _config;
        private readonly IAgentService _agentService;

        private Dictionary<string, Task?> _monitorTasks;
        private Dictionary<string, CancellationTokenSource?> _monitorCancellations;

        public MonitoringService(
            PerfomanceCalculator perfomanceCalc, 
            IRedisRepository redisRepository, 
            IAgentService agentService,
            IHubContext<NetworkHub> hubContext, 
            IConfiguration config)
        {
            _perfomanceCalc = perfomanceCalc;
            _redisRepository = redisRepository;
            _agentService = agentService;
            _hubContext = hubContext;
            _config = config;
        }

        public async Task SendAgentsAsync()
        {
            try
            {
             var agents = new Dictionary<string, string?>();

                var keys = GetAgents().Select(a => a.ToString()).ToList();
                foreach (var key in keys)
                {
                    var url = await _agentService.GetAgentUrlAsync(key);
                    agents.Add(key, url);
                }

                await _hubContext.Clients.All.SendAsync("ReceiveAgents", agents);
            }
            catch (NoSuchAgentException)
            {
                ;
            }
            catch (ReadingConfigurationException ex)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveError", ex.Message);
                Log.Logger.Error(ex.Message);
            }
            catch (NoAgentsException ex)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveError", ex.Message);
                Log.Logger.Error(ex.Message);
            }
        }

        public async Task SendHostnamesAsync()
        {
            try
            {
                var keys = GetAgents().Select(a => a.ToString()).ToList();

                await _hubContext.Clients.All.SendAsync("ReceiveHostnames", keys);
            }
            catch (ReadingConfigurationException ex)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveError", ex.Message);
                Log.Logger.Error(ex.Message);
            }
            catch (NoAgentsException ex)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveError", ex.Message);
                Log.Logger.Error(ex.Message);
            }
        }

        public async Task SendNetworkFromRequiredAgentAsync(string agent, int? count = null)
        {
            if (_monitorTasks.GetValueOrDefault(agent) == null || _monitorTasks.GetValueOrDefault(agent).IsCompleted)
            {
                int streamDelay;

                try
                {
                    streamDelay = GetStreamDelay();
                }
                catch (ReadingConfigurationException ex)
                {
                    streamDelay = 10;

                    await _hubContext.Clients.All.SendAsync("ReceiveError", ex.Message);
                    Log.Logger.Warning(ex.Message);
                }

                try
                {
                    var agentCancellation = _monitorCancellations.GetValueOrDefault(agent);
                    agentCancellation = new CancellationTokenSource();

                    await GetAndSendNetworkAsync(agent, streamDelay, count, agentCancellation.Token);
                }
                catch (RedisConnectionException)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveError", Error.NoConnectionToRedis);
                    Log.Logger.Error(Error.NoConnectionToRedis);
                }
                catch (Exception ex)
                {
                    await _hubContext.Clients.All.SendAsync("ReceiveError", $"{Error.Unexpected}: {ex.Message}");
                    Log.Logger.Error(ex.Message);
                }
            }           
        }

        public async Task StopRequiredAsync(string agent)
        {
            var agentTask = _monitorTasks.GetValueOrDefault(agent);
            var agentCancellation = _monitorCancellations.GetValueOrDefault(agent);

            if (agentTask != null)
            {
                agentCancellation!.Cancel();

                await agentTask;

                agentCancellation.Dispose();
            }
        }

        private IEnumerable<RedisKey> GetAgents()
        {
            string? host = _config.GetConnectionString("RedisConnection");
            if (string.IsNullOrEmpty(host))
                throw new ReadingConfigurationException(Error.FailedToReadRedisConnectionString);

            if (!int.TryParse(_config["RedisPort"], out var port))
                throw new ReadingConfigurationException(Error.FailedToReadRedisPort);

            var keys = _redisRepository.GetRedisKeys(host, port);
            if (!keys.Any())
                throw new NoAgentsException(Error.NoAgentsWereFound);

            return keys;
        }

        private async Task GetAndSendNetworkAsync(RedisKey agent, int streamReadDelay, int? count = null, CancellationToken cancellationToken = default)
        {
            var offset = StreamPosition.Beginning;
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var entries = await _redisRepository.ReadStreamAsync(agent, offset, count);
                    while (entries.Length == 0)
                    {
                        await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Поток по ключу {agent} пуст, ожидаются новые данные...", cancellationToken);
                        await Task.Delay(TimeSpan.FromSeconds(streamReadDelay));
                        entries = await _redisRepository.ReadStreamAsync(agent, offset, count);
                    }

                    Throughput? throughput = null;
                    ulong? delay = null;
                    Jitter? jitter = null;

                    var pTask = Task.Run(async () =>
                    {
                        var rawPackets = Deserializer.GetDeserializedRawPackets(entries);
                        foreach (var rp in rawPackets)
                        {
                            var packet = Packet.ParsePacket((LinkLayers)rp!.LinkLayerType, rp.Data);

                            if (throughput != null)
                            {
                                delay = _perfomanceCalc.GetTransmissionDelay(throughput.Bps, packet);
                                await _hubContext.Clients.All.SendAsync("ReceiveDelay", delay, cancellationToken);
                            }

                            var transport = PacketExtractor.ExtractTransport(packet);
                            var internet = PacketExtractor.ExtractInternet(packet);

                            if (_perfomanceCalc.Jitter == null)
                            {
                                if (internet is IPv4Packet)
                                {
                                    await _perfomanceCalc.FindJitterAsync((IPv4Packet)internet);
                                }                                 
                            }
                            else
                            {
                                jitter = _perfomanceCalc.Jitter;
                                await _hubContext.Clients.All.SendAsync("ReceiveJitter", jitter, cancellationToken);
                            }

                            if (transport != null)
                                await _hubContext.Clients.All.SendAsync("ReceivePacket", (Packet)transport, cancellationToken);
                            if (internet != null)
                                await _hubContext.Clients.All.SendAsync("ReceivePacket", (Packet)internet, cancellationToken); ;
                        }
                    });

                    var sTask = Task.Run(async () =>
                    {
                        var statistics = Deserializer.GetDeserializedStatistics(entries);
                        foreach (var s in statistics)
                        {
                            throughput = _perfomanceCalc.GetThroughput(s);
                            await _hubContext.Clients.All.SendAsync("ReceiveStatistics", statistics, cancellationToken);
                            await _hubContext.Clients.All.SendAsync("ReceiveThroughput", throughput, cancellationToken);
                        }
                    });

                    await Task.WhenAll(pTask, sTask);

                    offset = entries.Last().Id;
                }
                catch (RedisConnectionException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private int GetStreamDelay()
        {
            if (!int.TryParse(_config["StreamReadDelay"], out int streamReadDelay))
                throw new ReadingConfigurationException(Error.FailedToReadStreamReadDelay);

            return streamReadDelay;
        }
    }
} 
