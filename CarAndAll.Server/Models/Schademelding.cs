using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Schademelding
    {
        [Key]
        public int SchademeldingId { get; set; }

        [Required]
        public DateTime Datum { get; set; }

        [Required]
        public int HerstelPeriode { get; set; }

        [Required, MaxLength(500)]
        public string Beschrijving { get; set; }

        [Required]
        public int VoertuigId { get; set; }
        public Voertuig Voertuig { get; set; }

        [Required]
        public int VerhuuraanvraagId { get; set; }
        public Verhuuraanvraag Verhuuraanvraag { get; set; }

        [Required]
        public string MedewerkerId { get; set; }
        public Medewerker Medewerker { get; set; }

        public List<Notitie>? Notities { get; set; }
        public List<Foto>? Fotos { get; set; }
    }
}
