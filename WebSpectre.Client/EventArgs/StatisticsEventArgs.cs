using WebSpectre.Shared;

namespace WebSpectre.Client.EventArgs
{
    public class StatisticsEventArgs : System.EventArgs
    {
        public List<Statistics> Statistics { get; set; }
    }
}
