namespace WebSpectre.Shared.Agents
{
    public class HostInfo
    {
        public string OSVersion { get; set; }

        public Hardware Hardware { get; set; }

        public string[] IPAddresses { get; set; }

        public List<PcapDevice> AvailableDevices { get; set; }

        public bool IsCaptureProcessing { get; set; }
    }
}
