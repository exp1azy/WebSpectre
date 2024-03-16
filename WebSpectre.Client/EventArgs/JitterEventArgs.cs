using WebSpectre.Shared.Perfomance;

namespace WebSpectre.Client.EventArgs
{
    public class JitterEventArgs : System.EventArgs
    {
        public Jitter Jitter { get; set; }
    }
}
