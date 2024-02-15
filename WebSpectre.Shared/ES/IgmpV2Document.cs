namespace WebSpectre.Shared.ES
{
    public class IgmpV2Document : BasePacketDocument
    {
        public string Type { get; set; }

        public short Checksum { get; set; }

        public string GroupAddress { get; set; }

        public byte MaxResponseTime { get; set; }
    }
}
