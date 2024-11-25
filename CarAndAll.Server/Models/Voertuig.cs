using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Voertuig {
        [Key]
        public string Kenteken { get; set; }

        [Required, MaxLength(255)]
        public string Soort { get; set; }

        [Required, MaxLength(255)]
        public string Merk { get; set; }

        [Required, MaxLength(255)]
        public string Type { get; set; }

        [Required]
        public int Aanschafjaar { get; set; }

        public List<Verhuuraanvraag> Verhuuraanvragen { get; set; }

        public List<Schademelding> Schademeldingen { get; set; }
    }
}