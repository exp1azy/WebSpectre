namespace WebSpectre.Shared.Agents
{
    public class PcapDevice
    {
        public List<PcapAddress> Addresses { get; set; }

        public string Description { get; set; }

        public string FriendlyName { get; set; }

        public List<string> GatewayAddresses { get; set; }

        public string? MacAddress { get; set; }
    }
}
