using System;

namespace CarAndAll.Server.Models
{

    public class BedrijfsAbonnement
    {
        public required string Type { get; set; }
        public required decimal Prijs { get; set; }
    }
}