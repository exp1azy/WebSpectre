using WebSpectre.Shared;
using WebSpectre.Shared.Packets;

namespace WebSpectre.Server.Hubs.Interfaces
{
    public interface INetworkHub
    {
        public Task SendMessageAsync(string message, CancellationToken cancellationToken = default);

        public Task SendPacketAsync(BasePacket packet, CancellationToken cancellationToken = default);

        public Task SendStatisticsAsync(Statistics statistics, CancellationToken cancellationToken = default);
    }
}
