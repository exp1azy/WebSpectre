using WebSpectre.Shared.Agents;

namespace WebSpectre.Shared.Capture
{
    public class HostInfo
    {
        public string OSVersion { get; set; }

        public Hardware Hardware { get; set; }

        public string[] IPAddresses { get; set; }
    }
}
