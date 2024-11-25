using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Klant : Gebruiker
    {
        public Bedrijf Bedrijf { get; set; }
        public List<Verhuuraanvraag> Verhuuraanvraagen { get; set; }
    }
}