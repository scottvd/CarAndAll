using System;

namespace CarAndAll.Server.Models
{
    public class Voertuig
    {
        public required string Soort { get; set; }
        public required string Merk { get; set; }
        public required string Type { get; set; }
        public required string Kenteken { get; set; }
        public required string Aanschafjaar { get; set; }
    }
}