using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAndAll.Server.Models
{
    public class Voertuig {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoertuigId { get; set; }

        [Required, MaxLength(10)]
        public string Kenteken { get; set; }

        [Required, MaxLength(50)]
        public string Soort { get; set; }

        [Required, MaxLength(50)]
        public string Merk { get; set; }

        [Required, MaxLength(50)]
        public string Type { get; set; }

        [Required]
        [Range(1900, int.MaxValue, ErrorMessage = "Ongeldig aanschafjaar.")]
        public int Aanschafjaar { get; set; }

        [Required]
        public decimal Prijs { get; set; }

        [Required]
        public bool IsActief { get; set; }

        public List<Verhuuraanvraag>? Verhuuraanvragen { get; set; }

        public List<Schademelding>? Schademeldingen { get; set; }

        public Voertuig() {
            IsActief = true;
        }
    }
}