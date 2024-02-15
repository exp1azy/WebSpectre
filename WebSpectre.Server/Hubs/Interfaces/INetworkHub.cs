using PacketDotNet;
using StackExchange.Redis;
using WebSpectre.Server.Entities;
using WebSpectre.Shared;

namespace WebSpectre.Server.Hubs.Interfaces
{
    public interface INetworkHub
    {
        public Task SendMessageAsync(string message, CancellationToken cancellationToken = default);

        public Task SendPacketAsync(Packet packet, CancellationToken cancellationToken = default);

        public Task SendStatisticsAsync(Statistics statistics, CancellationToken cancellationToken = default);

        public Task SendAgentsAsync(IEnumerable<RedisKey> agents, CancellationToken cancellationToken = default);

        public Task SendCurrentThroughputAsync(Throughput throughput, CancellationToken cancellationToken = default);

        public Task SendCurrentDelayAsync(ulong delay, CancellationToken cancellationToken = default);

        public Task SendCurrentJitterAsync(Jitter jitter, CancellationToken cancellationToken = default);
    }
}
