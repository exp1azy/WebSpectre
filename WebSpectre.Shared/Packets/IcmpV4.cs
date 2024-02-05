namespace WebSpectre.Shared.Packets
{
    public class IcmpV4 : BasePacket
    {
        public ushort Checksum { get; set; }

        public string Data { get; set; }

        public ushort IcmpV4Id { get; set; }

        public ushort Sequence { get; set; }

        public string TypeCode { get; set; }

        public bool ValidIcmpChecksum { get; set; }
    }
}
