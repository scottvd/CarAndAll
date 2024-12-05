using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Verhuuraanvraag {
        [Key]
        public int AanvraagID { get; set; }

        [Required]
        public DateTime StartDatum { get; set; }

        [Required]
        public DateTime EindDatum { get; set; }

        [Required, MaxLength(255)]
        public string Status { get; set; }

        [Required]
        public Klant Klant { get; set; }

        [Required]
        public Voertuig Voertuig { get; set; }

        public List<Schademelding> Schademeldingen { get; set; }
    }
}