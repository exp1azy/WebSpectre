using System.Text;

namespace WebSpectre.Shared.ES
{
    public class IPv6Document : BasePacketDocument
    {
        public string DestinationAddress { get; set; }

        public int HeaderLength { get; set; }

        public int HopLimit { get; set; }

        public ushort PayloadLength { get; set; }

        public string Protocol { get; set; }

        public string SourceAddress { get; set; }

        public int TimeToLive { get; set; }

        public int TotalLength { get; set; }

        public string Version { get; set; }

        public List<IPv6ExtensionHeader> ExtensionHeaders { get; set; }

        public int ExtensionHeadersLength { get; set; }

        public int FlowLabel { get; set; }

        public string NextHeader { get; set; }

        public int TrafficClass { get; set; }
    }

    public class IPv6ExtensionHeader
    {
        public string Header { get; set; }

        public int HeaderExtensionLength { get; set; }

        public ushort Length { get; set; }

        public string NextHeader { get; set; }

        public string Payload { get; set; }

        public static explicit operator IPv6ExtensionHeader(PacketDotNet.IPv6ExtensionHeader h) => new IPv6ExtensionHeader
        {
            Header = h.Header.ToString(),
            Length = h.Length,
            HeaderExtensionLength = h.HeaderExtensionLength,
            NextHeader = h.NextHeader.ToString(),
            Payload = Encoding.UTF8.GetString(h.Payload.ActualBytes())
        };
    }
}
