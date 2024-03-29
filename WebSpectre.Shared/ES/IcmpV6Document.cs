﻿namespace WebSpectre.Shared.ES
{
    public class IcmpV6Document : BasePacketDocument
    {
        public ushort Checksum { get; set; }

        public byte Code { get; set; }

        public string Type { get; set; }

        public bool ValidIcmpChecksum { get; set; }
    }
}
