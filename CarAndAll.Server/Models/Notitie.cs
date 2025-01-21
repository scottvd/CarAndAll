using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Notitie
    {
        [Key]
        public int NotitieId { get; set; }

        [Required, MaxLength(500)]
        public string Inhoud { get; set; }

        [Required]
        public DateTime Datum { get; set; }

        [Required]
        public int SchademeldingId { get; set; }
        public Schademelding Schademelding { get; set; }

        [Required]
        public string MedewerkerId { get; set; }
        public Medewerker Medewerker { get; set; }
    }
}
