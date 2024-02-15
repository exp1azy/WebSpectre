using Newtonsoft.Json;
using StackExchange.Redis;

namespace WebSpectre.Shared.Services
{
    /// <summary>
    /// Десериализатор записей потока Redis. 
    /// </summary>
    public static class Deserializer
    {
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
    }
}
