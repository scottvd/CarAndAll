using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public enum KlantType
    {
        Particulier,
        ZakelijkeHuurder,
        ZakelijkeBeheerder // Wagenparkbeheerder, of hoe je het ook wilt noemen.
    }

    public class Klant : Gebruiker
    {
        [Required, MaxLength(255)]
        public string Naam { get; set; }

        [Required, MaxLength(255)]
        public string Adres { get; set; }

        // Veld om het type klant aan te geven (particulier of zakelijk)
        [Required]
        public KlantType Type { get; set; } // Bijvoorbeeld "Particulier" of "Zakelijk"

        public int? BedrijfId { get; set; }
        public Bedrijf? Bedrijf { get; set; } // Alleen ingevuld voor zakelijke klanten en zakelijke huurders

        public List<Verhuuraanvraag>? Verhuuraanvraagen { get; set; }
    }
}