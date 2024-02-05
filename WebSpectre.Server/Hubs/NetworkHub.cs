using Microsoft.AspNetCore.SignalR;
using WebSpectre.Server.Hubs.Interfaces;
using WebSpectre.Shared;
using WebSpectre.Shared.Packets;

namespace WebSpectre.Server.Hubs
{
    public class NetworkHub : Hub, INetworkHub
    {
        public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", message, cancellationToken);
        }

        public async Task SendPacketAsync(BasePacket packet, CancellationToken cancellationToken = default)
        {
            await Clients.Caller.SendAsync("ReceivePacket", packet, cancellationToken);
        }

        public async Task SendStatisticsAsync(Statistics statistics, CancellationToken cancellationToken = default)
        {
            await Clients.Caller.SendAsync("ReceiveStatistics", statistics, cancellationToken);
        }
    }
}
