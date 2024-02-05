using Newtonsoft.Json;

namespace WebSpectre.Shared
{
    public class Statistics
    {
        [JsonProperty("Device")]
        public Device Device { get; set; }

        [JsonProperty("Timeval")]
        public Timeval Timeval { get; set; }

        [JsonProperty("ReceivedPackets")]
        public long ReceivedPackets { get; set; }

        [JsonProperty("ReceivedBytes")]
        public long ReceivedBytes { get; set; }
    }
}
