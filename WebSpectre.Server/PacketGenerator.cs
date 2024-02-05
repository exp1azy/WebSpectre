using Newtonsoft.Json;
using PacketDotNet;
using StackExchange.Redis;
using System.Text;
using WebSpectre.Shared;
using WebSpectre.Shared.Packets;

namespace PacketDataIndexer
{
    internal static class PacketGenerator
    {
        /// <summary>
        /// Распаковка и десериализация данных о <see cref="RawPacket"/> из Redis.
        /// </summary>
        /// <returns>Список <see cref="RawPacket"/></returns>
        public static List<RawPacket?> GetDeserializedRawPackets(StreamEntry[] entries)
        {
            var allBatches = entries.Select(e => e.Values.First());
            var rawPacketsBatches = allBatches.Where(v => v.Name.StartsWith("raw_packets"));

            var rawPackets = rawPacketsBatches.Select(b => JsonConvert.DeserializeObject<RawPacket>(b.Value.ToString())).ToList();

            return rawPackets;
        }

        /// <summary>
        /// Распаковка и десериализация данных о <see cref="Statistics"/> из Redis.
        /// </summary>
        /// <returns>Список <see cref="Statistics"/></returns>
        public static List<Statistics?> GetDeserializedStatistics(StreamEntry[] entries)
        {
            var allBatches = entries.Select(e => e.Values.First());
            var statisticsBatches = allBatches.Where(v => v.Name.StartsWith("statistics"));

            var statistics = statisticsBatches.Select(b => JsonConvert.DeserializeObject<Statistics>(b.Value.ToString())).ToList();

            return statistics;
        }          

        /// <summary>
        /// Формирование документа <see cref="BasePacket"/>, содержащий пакет транспортного уровня, для ElasticSearch.
        /// </summary>
        /// <param name="transport">Неизвлеченный пакет транспортного уровня..</param>
        /// <param name="transportId">Идентификатор для пакета транспортного уровня.</param>
        /// <param name="networkId">Идентификатор для пакета сетевого уровня.</param>
        /// <param name="agent">Агент.</param>
        /// <returns>Извлеченный пакет.</returns>
        public static BasePacket? GenerateTransportPacket(Packet packet)
        {
            var transport = packet.Extract<TcpPacket>() ?? (object)packet.Extract<UdpPacket>();

            if (transport is TcpPacket)
            {
                TcpPacket tcp = (TcpPacket)transport;

                return new Tcp
                {
                    Acknowledgment = tcp.Acknowledgment,
                    AcknowledgmentNumber = tcp.AcknowledgmentNumber,
                    Bytes = Encoding.UTF8.GetString(tcp.Bytes),
                    Checksum = tcp.Checksum,
                    Color = tcp.Color,
                    CongestionWindowReduced = tcp.CongestionWindowReduced,
                    DataOffset = tcp.DataOffset,
                    DestinationPort = tcp.DestinationPort,
                    ExplicitCongestionNotificationEcho = tcp.ExplicitCongestionNotificationEcho,
                    Finished = tcp.Finished,
                    Flags = tcp.Flags,
                    HasPayloadData = tcp.HasPayloadData,
                    HasPayloadPacket = tcp.HasPayloadPacket,
                    HeaderData = Encoding.UTF8.GetString(tcp.HeaderData),
                    IsPayloadInitialized = tcp.IsPayloadInitialized,
                    NonceSum = tcp.NonceSum,
                    Options = Encoding.UTF8.GetString(tcp.Options),
                    OptionsCollection = tcp.OptionsCollection == null ? null : tcp.OptionsCollection.Select(o => (TcpOption)o).ToList(),
                    OptionsSegment = Encoding.UTF8.GetString(tcp.OptionsSegment.ActualBytes()),
                    PayloadData = Encoding.UTF8.GetString(tcp.PayloadData),
                    Push = tcp.Push,
                    Reset = tcp.Reset,
                    SequenceNumber = tcp.SequenceNumber,
                    SourcePort = tcp.SourcePort,
                    Synchronize = tcp.Synchronize,
                    TotalPacketLength = tcp.TotalPacketLength,
                    Urgent = tcp.Urgent,
                    UrgentPointer = tcp.UrgentPointer,
                    ValidChecksum = tcp.ValidChecksum,
                    ValidTcpChecksum = tcp.ValidTcpChecksum,
                    WindowSize = tcp.WindowSize,
                };
            }
            else if (transport is UdpPacket)
            {
                UdpPacket udp = (UdpPacket)transport;

                return new Udp
                {
                    Bytes = Encoding.UTF8.GetString(udp.Bytes),
                    Checksum = udp.Checksum,
                    Color = udp.Color,
                    DestinationPort = udp.DestinationPort,
                    HasPayloadData = udp.HasPayloadData,
                    HasPayloadPacket = udp.HasPayloadPacket,
                    HeaderData = Encoding.UTF8.GetString(udp.HeaderData),
                    IsPayloadInitialized = udp.IsPayloadInitialized,
                    Length = udp.Length,
                    PayloadData = Encoding.UTF8.GetString(udp.PayloadData),
                    SourcePort = udp.SourcePort,
                    TotalPacketLength = udp.TotalPacketLength,
                    ValidChecksum = udp.ValidChecksum,
                    ValidUdpChecksum = udp.ValidUdpChecksum,
                };
            }
            else return null;
        }

        /// <summary>
        /// Формирование документа <see cref="BasePacket"/>, содержащий пакет сетевого уровня, для ElasticSearch.
        /// </summary>
        /// <param name="network">Неизвлеченный пакет сетевого уровня.</param>
        /// <param name="networkId">Идентификатор пакета сетевого уровня.</param>
        /// <param name="transportId">Идентификатор пакета транспортного уровня.</param>
        /// <param name="agent">Агент.</param>
        /// <returns>Извлеченный пакет.</returns>
        public static BasePacket? GenerateNetworkPacket(Packet packet)
        {
            var network = packet.Extract<IcmpV4Packet>() ?? packet.Extract<IcmpV6Packet>() ??
            packet.Extract<IgmpV2Packet>() ?? packet.Extract<IPv4Packet>() ?? (object)packet.Extract<IPv6Packet>();

            if (network is IPv4Packet)
            {
                IPv4Packet ipv4 = (IPv4Packet)network;

                return new IPv4
                {
                    Bytes = Encoding.UTF8.GetString(ipv4.Bytes),
                    HasPayloadData = ipv4.HasPayloadData,
                    HasPayloadPacket = ipv4.HasPayloadPacket,
                    HeaderData = Encoding.UTF8.GetString(ipv4.HeaderData),
                    PayloadData = Encoding.UTF8.GetString(ipv4.PayloadData),
                    IsPayloadInitialized = ipv4.IsPayloadInitialized,
                    TotalPacketLength = ipv4.TotalPacketLength,
                    Checksum = ipv4.Checksum,
                    Color = ipv4.Color,
                    DestinationAddress = ipv4.DestinationAddress.ToString(),
                    DifferentiatedServices = ipv4.DifferentiatedServices,
                    FragmentFlags = ipv4.FragmentFlags,
                    FragmentOffset = ipv4.FragmentOffset,
                    HeaderLength = ipv4.HeaderLength,
                    HopLimit = ipv4.HopLimit,
                    IPv4Id = ipv4.Id,
                    PayloadLength = ipv4.PayloadLength,
                    Protocol = ipv4.Protocol.ToString(),
                    SourceAddress = ipv4.SourceAddress.ToString(),
                    TimeToLive = ipv4.TimeToLive,
                    TotalLength = ipv4.TotalLength,
                    TypeOfService = ipv4.TypeOfService,
                    ValidChecksum = ipv4.ValidChecksum,
                    ValidIPChecksum = ipv4.ValidIPChecksum,
                    Version = ipv4.Version.ToString()
                };
            }
            else if (network is IPv6Packet)
            {
                IPv6Packet ipv6 = (IPv6Packet)network;

                return new IPv6
                {
                    Bytes = Encoding.UTF8.GetString(ipv6.Bytes),
                    HasPayloadData = ipv6.HasPayloadData,
                    HasPayloadPacket = ipv6.HasPayloadPacket,
                    HeaderData = Encoding.UTF8.GetString(ipv6.HeaderData),
                    PayloadData = Encoding.UTF8.GetString(ipv6.PayloadData),
                    IsPayloadInitialized = ipv6.IsPayloadInitialized,
                    TotalPacketLength = ipv6.TotalPacketLength,
                    Color = ipv6.Color,
                    DestinationAddress = ipv6.DestinationAddress.ToString(),
                    ExtensionHeaders = ipv6.ExtensionHeaders.Select(h => (WebSpectre.Shared.Packets.IPv6ExtensionHeader)h).ToList(),
                    ExtensionHeadersLength = ipv6.ExtensionHeadersLength,
                    FlowLabel = ipv6.FlowLabel,
                    HeaderLength = ipv6.HeaderLength,
                    HopLimit = ipv6.HopLimit,
                    NextHeader = ipv6.NextHeader.ToString(),
                    PayloadLength = ipv6.PayloadLength,
                    Protocol = ipv6.Protocol.ToString(),
                    SourceAddress = ipv6.SourceAddress.ToString(),
                    TimeToLive = ipv6.TimeToLive,
                    TotalLength = ipv6.TotalLength,
                    TrafficClass = ipv6.TrafficClass,
                    Version = ipv6.Version.ToString()
                };
            }
            else if (network is IcmpV4Packet)
            {
                IcmpV4Packet icmpv4 = (IcmpV4Packet)network;

                return new IcmpV4
                {
                    Bytes = Encoding.UTF8.GetString(icmpv4.Bytes),
                    HasPayloadData = icmpv4.HasPayloadData,
                    HeaderData = Encoding.UTF8.GetString(icmpv4.HeaderData),
                    HasPayloadPacket = icmpv4.HasPayloadPacket,
                    IsPayloadInitialized = icmpv4.IsPayloadInitialized,
                    PayloadData = Encoding.UTF8.GetString(icmpv4.PayloadData),
                    TotalPacketLength = icmpv4.TotalPacketLength,
                    Color = icmpv4.Color,
                    Checksum = icmpv4.Checksum,
                    TypeCode = icmpv4.TypeCode.ToString(),
                    Data = Encoding.UTF8.GetString(icmpv4.Data),
                    IcmpV4Id = icmpv4.Id,
                    Sequence = icmpv4.Sequence,
                    ValidIcmpChecksum = icmpv4.ValidIcmpChecksum
                };
            }
            else if (network is IcmpV6Packet)
            {
                IcmpV6Packet icmpv6 = (IcmpV6Packet)network;

                return new IcmpV6
                {
                    Bytes = Encoding.UTF8.GetString(icmpv6.Bytes),
                    HasPayloadData = icmpv6.HasPayloadData,
                    HasPayloadPacket = icmpv6.HasPayloadPacket,
                    HeaderData = Encoding.UTF8.GetString(icmpv6.HeaderData),
                    IsPayloadInitialized = icmpv6.IsPayloadInitialized,
                    PayloadData = Encoding.UTF8.GetString(icmpv6.PayloadData),
                    TotalPacketLength = icmpv6.TotalPacketLength,
                    Color = icmpv6.Color,
                    Checksum = icmpv6.Checksum,
                    Code = icmpv6.Code,
                    ValidIcmpChecksum = icmpv6.ValidIcmpChecksum,
                    Type = icmpv6.Type.ToString()
                };
            }
            else if (network is IgmpV2Packet)
            {
                IgmpV2Packet igmp = (IgmpV2Packet)network;

                return new IgmpV2
                {
                    Bytes = Encoding.UTF8.GetString(igmp.Bytes),
                    Checksum = igmp.Checksum,
                    Color = igmp.Color,
                    GroupAddress = igmp.GroupAddress.ToString(),
                    HasPayloadData = igmp.HasPayloadData,
                    HasPayloadPacket = igmp.HasPayloadPacket,
                    HeaderData = Encoding.UTF8.GetString(igmp.HeaderData),
                    IsPayloadInitialized = igmp.IsPayloadInitialized,
                    MaxResponseTime = igmp.MaxResponseTime,
                    PayloadData = Encoding.UTF8.GetString(igmp.PayloadData),
                    TotalPacketLength = igmp.TotalPacketLength,
                    Type = igmp.Type.ToString()
                };
            }
            else return null;
        } 
    }
}
