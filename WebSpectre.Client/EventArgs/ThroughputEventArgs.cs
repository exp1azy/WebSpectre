using WebSpectre.Shared.Perfomance;

namespace WebSpectre.Client.EventArgs
{
    public class ThroughputEventArgs : System.EventArgs
    {
        public Throughput Throughput { get; set; }
    }
}
