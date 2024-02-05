namespace WebSpectre.Shared.Packets
{
    public class IcmpV6 : BasePacket
    {
        public ushort Checksum { get; set; }

        public byte Code { get; set; }

        public string Type { get; set; }

        public bool ValidIcmpChecksum { get; set; }
    }
}
