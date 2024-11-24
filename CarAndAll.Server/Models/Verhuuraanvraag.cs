using System;

namespace CarAndAll.Server.Models
{
    public class Verhuuraanvraag
    {
        private DateTime Startdatum { get; set; }
        private DateTime Einddatum { get; set; }
        public required string Status { get; set; }
        public required Voertuig Voertuig { get; set; }

    }
}