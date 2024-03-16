using PacketDotNet;

namespace WebSpectre.Client.EventArgs
{
    public class PacketEventArgs : System.EventArgs
    {
        public Packet Packet { get; set; }
    }
}
