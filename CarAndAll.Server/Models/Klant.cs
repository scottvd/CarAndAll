using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Klant
    {
        [Key]
        public int KlantID { get; set; }

        [Required, MaxLength(255)]
        public string Naam { get; set; }

        [Required, MaxLength(255)]
        public string Adres { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; }

        public Bedrijf Bedrijf { get; set; }
        public List<Verhuuraanvraag> Verhuuraanvraagen { get; set; }
    }
}