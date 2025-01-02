using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Klant : Gebruiker
    {
        [Required, MaxLength(255)]
        public string Naam { get; set; }

        [Required, MaxLength(255)]
        public string Adres { get; set; }

        public int? BedrijfId { get; set; }
        public Bedrijf? Bedrijf { get; set; }
        public List<Verhuuraanvraag>? Verhuuraanvraagen { get; set; }
    }
}