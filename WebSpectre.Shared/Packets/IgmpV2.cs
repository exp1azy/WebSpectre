namespace WebSpectre.Shared.Packets
{
    public class IgmpV2 : BasePacket
    {
        public string Type { get; set; }

        public short Checksum { get; set; }

        public string GroupAddress { get; set; }

        public byte MaxResponseTime { get; set; }
    }
}
