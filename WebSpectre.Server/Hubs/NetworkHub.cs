using Microsoft.AspNetCore.SignalR;
using PacketDotNet;
using StackExchange.Redis;
using WebSpectre.Server.Entities;
using WebSpectre.Server.Hubs.Interfaces;
using WebSpectre.Shared;

namespace WebSpectre.Server.Hubs
{
    public class NetworkHub : Hub, INetworkHub
    {
        public async Task SendAgentsAsync(IEnumerable<RedisKey> agents, CancellationToken cancellationToken = default)
        {
            await Clients.Caller.SendAsync("ReceiveAgents", agents, cancellationToken);
        }

        public async Task SendCurrentDelayAsync(ulong delay, CancellationToken cancellationToken = default)
        {
            await Clients.Caller.SendAsync("ReceiveDelay", delay, cancellationToken);
        }

        public async Task SendCurrentJitterAsync(Jitter jitter, CancellationToken cancellationToken = default)
        {
            await Clients.Caller.SendAsync("ReceiveJitter", jitter, cancellationToken);
        }

        public async Task SendCurrentThroughputAsync(Throughput throughput, CancellationToken cancellationToken = default)
        {
            await Clients.Caller.SendAsync("ReceiveThroughput", throughput, cancellationToken);
        }

        public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", message, cancellationToken);
        }

        public async Task SendPacketAsync(Packet packet, CancellationToken cancellationToken = default)
        {
            await Clients.Caller.SendAsync("ReceivePacket", packet, cancellationToken);
        }

        public async Task SendStatisticsAsync(Statistics statistics, CancellationToken cancellationToken = default)
        {
            await Clients.Caller.SendAsync("ReceiveStatistics", statistics, cancellationToken);
        }
    }
}
