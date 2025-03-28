using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public enum HuurderType
    {
        Particulier,
        Zakelijk,
        Beheerder
    }

    public class Huurder : Gebruiker
    {
        [MaxLength(255)]
        public string Adres { get; set; }

        [Required]
        public HuurderType Type { get; set; } 

        public int? BedrijfId { get; set; }
        public Bedrijf? Bedrijf { get; set; }

        public List<Verhuuraanvraag>? Verhuuraanvraagen { get; set; }

        public int? VerwijderingsverzoekId { get; set; }
        public Verwijderingsverzoek? Verwijderingsverzoek { get; set; }
    }
}