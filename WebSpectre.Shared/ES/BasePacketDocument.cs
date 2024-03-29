﻿namespace WebSpectre.Shared.ES
{
    public class BasePacketDocument
    {
        public Guid Id { get; set; }

        public Guid? Nested { get; set; } 

        public string Agent { get; set; }

        public string Model { get; set; }

        public byte[] Bytes { get; set; }

        public byte[] HeaderData { get; set; }

        public byte[] PayloadData { get; set; }

        public int TotalPacketLength { get; set; }

        public string Color { get; set; }

        public bool HasPayloadData { get; set; }

        public bool HasPayloadPacket { get; set; }

        public bool IsPayloadInitialized { get; set; }
    }
}
