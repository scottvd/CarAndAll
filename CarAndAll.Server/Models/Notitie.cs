using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Notitie
    {
        [Key]
        public int NotitieID { get; set; }

        [Required, MaxLength(500)] // Meer ruimte voor langere notities
        public string Inhoud { get; set; }

        [Required]
        public DateTime Datum { get; set; } // Datum wanneer de notitie is toegevoegd

        [Required]
        public int SchademeldingId { get; set; } // Foreign key naar Schademelding
        public Schademelding Schademelding { get; set; }

        [Required]
        public int MedewerkerId { get; set; } // Foreign key naar Medewerker
        public Medewerker Medewerker { get; set; }
    }
}
