namespace WebSpectre.Shared.Packets
{
    public class BasePacket
    {
        public string Bytes { get; set; }

        public string HeaderData { get; set; }

        public string PayloadData { get; set; }

        public int TotalPacketLength { get; set; }

        public string Color { get; set; }

        public bool HasPayloadData { get; set; }

        public bool HasPayloadPacket { get; set; }

        public bool IsPayloadInitialized { get; set; }
    }
}
