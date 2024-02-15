using PacketDotNet;
using System.Net.NetworkInformation;
using WebSpectre.Server.Entities;
using WebSpectre.Shared;

namespace WebSpectre.Server.Services
{
    public class PerfomanceCalculator
    {
        private ulong _oldSec = 0;
        private ulong _oldUsec = 0;
        private int _capacity;
        private List<long> _packetTransitTimes;
        private Jitter _jitter;

        public PerfomanceCalculator(IConfiguration config)
        {
            if (int.TryParse(config["PacketTransitTimeCapacity"], out int capacity))
                _capacity = capacity;
            else            
                _capacity = 20;
                       
            _packetTransitTimes = new List<long>(_capacity);
        }

        public Jitter Jitter => _jitter;

        public ulong GetTransmissionDelay(ulong bps, Packet packet) => (ulong)packet.Bytes.Length * 8 / bps;

        public Throughput GetThroughput(Statistics s)
        {
            ulong delay = (s.Timeval.Seconds - _oldSec) * 1000000 - _oldUsec + s.Timeval.MicroSeconds;

            ulong bps = ((ulong)s.ReceivedBytes * 8 * 1000000) / delay;
            var ts = s.Timeval.Date.ToLongTimeString();

            _oldSec = s.Timeval.Seconds;
            _oldUsec = s.Timeval.MicroSeconds;

            return new Throughput { Bps = bps, Ts = ts };
        }

        public async Task FindJitterAsync(IPv4Packet packet)
        {
            if (_packetTransitTimes.Count == _capacity)
            {
                var minTransitTime = _packetTransitTimes.Min();
                var avgTransitTime = (long)_packetTransitTimes.Average();
                var maxTransitTime = _packetTransitTimes.Max();

                _packetTransitTimes.Clear();

                _jitter = new Jitter
                {
                    MaxDifference = maxTransitTime - avgTransitTime,
                    MinDifference = avgTransitTime - minTransitTime
                };
            }
            else
            {
                if (!packet.SourceAddress.ToString().StartsWith("192.168"))
                {
                    var ping = new Ping();
                    var reply = ping.Send(packet.SourceAddress);

                    if (reply.Status == IPStatus.Success)
                        _packetTransitTimes.Add(reply.RoundtripTime);
                }
            }          
        }
    }
}
