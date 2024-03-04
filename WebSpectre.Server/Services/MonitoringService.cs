using Microsoft.AspNetCore.SignalR;
using PacketDotNet;
using StackExchange.Redis;
using WebSpectre.Server.Entities;
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

        public MonitoringService(
            PerfomanceCalculator perfomanceCalc, 
            IRedisRepository redisRepository, 
            IHubContext<NetworkHub> hubContext, 
            IConfiguration config)
        {
            _perfomanceCalc = perfomanceCalc;
            _redisRepository = redisRepository;
            _hubContext = hubContext;
            _config = config;
        }

        public async Task<List<string>> GetAgentsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var keys = GetAgents().Select(a => a.ToString()).ToList();

               return keys;
            }
            catch (ReadingConfigurationException)
            {
                throw;
            }
            catch (NoAgentsException)
            {
                throw;
            }
        }

        public async Task SendNetworkFromStreamAsync(string agent, int count, CancellationToken cancellationToken)
        {
            if (!int.TryParse(_config["StreamReadDelay"], out int streamReadDelay))
                throw new ReadingConfigurationException(Error.FailedToReadStreamReadDelay);

            try
            {
                await GetAndSendNetworkAsync(agent, count, streamReadDelay, cancellationToken);
            }
            catch (WebSpectreRedisConnectionException)
            {
                throw;
            }
            catch (ApplicationException)
            {
                throw;
            }
        }

        public async Task SendNetworkFromStreamAsync(int count, CancellationToken cancellationToken)
        {
            if (!int.TryParse(_config["StreamReadDelay"], out int streamReadDelay))
                throw new ReadingConfigurationException(Error.FailedToReadStreamReadDelay);

            IEnumerable<RedisKey> keys;

            try
            {
                keys = GetAgents();
            }
            catch (ReadingConfigurationException)
            {
                throw;
            }
            catch (NoAgentsException)
            {
                throw;
            }

            var tasks = new List<Task>();

            foreach (var key in keys)
            {
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        await GetAndSendNetworkAsync(key, count, streamReadDelay, cancellationToken);
                    }
                    catch (WebSpectreRedisConnectionException)
                    {
                        throw;
                    }
                    catch (ApplicationException)
                    {
                        throw;
                    }
                }));
            }

            await Task.WhenAll(tasks);
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

        private async Task GetAndSendNetworkAsync(RedisKey agent, int count, int streamReadDelay, CancellationToken cancellationToken)
        {
            var offset = StreamPosition.Beginning;
            while (true)
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
                catch (RedisConnectionException ex)
                {
                    throw new WebSpectreRedisConnectionException(Error.NoConnectionToRedis, ex);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(Error.Unexpected, ex);
                }
            }
        }
    }
} 
