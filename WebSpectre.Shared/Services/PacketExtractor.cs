using PacketDotNet;

namespace WebSpectre.Shared.Services
{
    public static class PacketExtractor
    {
        public static object? ExtractTransport(Packet packet) =>
            packet.Extract<TcpPacket>() ?? (object)packet.Extract<UdpPacket>();

        public static object? ExtractInternet(Packet packet) =>
            packet.Extract<IcmpV4Packet>() ?? packet.Extract<IcmpV6Packet>() ??
            packet.Extract<IgmpV2Packet>() ?? packet.Extract<Ieee8021QPacket>() ??
            packet.Extract<IPv4Packet>() ?? (object)packet.Extract<IPv6Packet>();
    }
}
