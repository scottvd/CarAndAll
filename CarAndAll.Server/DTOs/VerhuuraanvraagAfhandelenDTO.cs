using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class VerhuuraanvraagAfhandelenDTO
    {
        public int VerhuuraanvraagId { get; set; }

        public int? HerstelPeriode { get; set; }

        public string? Beschrijving { get; set; }
    }
}
