namespace WebSpectre.Shared.Capture
{
    public class PcapAddress
    {
        public string? Address { get; set; }

        public string? Netmask { get; set; }

        public string? BroadAddr { get; set; }

        public string? DestAddr { get; set; }

        public static explicit operator PcapAddress(SharpPcap.LibPcap.PcapAddress pcapAddress) => new PcapAddress
        {
            Address = pcapAddress.Addr == null ? null : pcapAddress.Addr.ToString(),
            BroadAddr = pcapAddress.Broadaddr == null ? null : pcapAddress.Broadaddr.ToString(),
            DestAddr = pcapAddress.Dstaddr == null ? null : pcapAddress.Dstaddr.ToString(),
            Netmask = pcapAddress.Netmask == null ? null : pcapAddress.Netmask.ToString()
        };
        
    }
}
