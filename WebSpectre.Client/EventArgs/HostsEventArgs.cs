using WebSpectre.Shared.Agents;

namespace WebSpectre.Client.EventArgs
{
    public class HostsEventArgs : System.EventArgs
    {
        public Dictionary<string, HostInfo?> Hosts { get; set; }
    }
}
