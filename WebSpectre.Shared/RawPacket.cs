using Newtonsoft.Json;

namespace WebSpectre.Shared
{
    public class RawPacket
    {
        [JsonProperty("Data")]
        public byte[] Data { get; set; }

        [JsonProperty("LinkLayerType")]
        public ushort LinkLayerType { get; set; }

        [JsonProperty("Timeval")]
        public Timeval Timeval { get; set; }

        [JsonProperty("PacketLength")]
        public int PacketLength { get; set; }
    }
}
