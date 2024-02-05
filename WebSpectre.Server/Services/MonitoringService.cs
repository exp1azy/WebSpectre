using PacketDataIndexer;
using PacketDotNet;
using StackExchange.Redis;
using WebSpectre.Server.Exceptions;
using WebSpectre.Server.Hubs.Interfaces;
using WebSpectre.Server.Repositories.Interfaces;
using WebSpectre.Server.Resources;
using WebSpectre.Server.Services.Interfaces;

namespace WebSpectre.Server.Services
{
    public class MonitoringService : IMonitoringService
    {
        private readonly IRedisRepository _redisRepository;
        private readonly INetworkHub _hubContext;
        private readonly IConfiguration _config;

        public MonitoringService(IRedisRepository redisRepository, INetworkHub hubContext, IConfiguration config)
        {
            _redisRepository = redisRepository;
            _hubContext = hubContext;
            _config = config;
        }

        public async Task ReadStreamAsync(string agent, int count, CancellationToken cancellationToken)
        {
            if (!int.TryParse(_config["StreamReadDelay"], out int streamReadDelay))          
                throw new ArgumentNullException(Error.FailedToReadStreamReadDelay);
            
            var offset = StreamPosition.Beginning;
            while (true)
            {
                try
                {
                    var entries = await _redisRepository.ReadStreamAsync(agent, offset, count);
                    while (entries.Length == 0)
                    {
                        await _hubContext.SendMessageAsync($"Поток по ключу {agent} пуст, ожидаются новые данные...", cancellationToken);
                        await Task.Delay(TimeSpan.FromSeconds(streamReadDelay));
                        entries = await _redisRepository.ReadStreamAsync(agent, offset, count);
                    }

                    var rawPackets = PacketGenerator.GetDeserializedRawPackets(entries);
                    foreach (var rp in rawPackets)
                    {
                        var packet = Packet.ParsePacket((LinkLayers)rp!.LinkLayerType, rp.Data);

                        var transport = PacketGenerator.GenerateTransportPacket(packet);
                        var network = PacketGenerator.GenerateNetworkPacket(packet);

                        if (transport != null)
                            await _hubContext.SendPacketAsync(transport, cancellationToken);
                        if (network != null)
                            await _hubContext.SendPacketAsync(network, cancellationToken);
                    }

                    var statistics = PacketGenerator.GetDeserializedStatistics(entries);
                    foreach (var s in statistics)
                    {
                        await _hubContext.SendStatisticsAsync(s, cancellationToken);
                    }

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

        public async Task ReadStreamAsync(int count, CancellationToken cancellationToken)
        {
            
        }
    }
}
