using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public enum SchadeStatus
    {
        Open,
        InBehandeling,
        Afgerond
    }
    public class Schademelding
    {
        [Key]
        public int SchademeldingID { get; set; }

        [Required, MaxLength(50)]
        public SchadeStatus Status { get; set; } // Bijv. "Open", "In Behandeling", "Afgerond"

        [Required]
        public DateTime Datum { get; set; }

        [Required, MaxLength(500)]
        public string Beschrijving { get; set; }

        [Required]
        public int VoertuigId { get; set; }
        public Voertuig Voertuig { get; set; }

        [Required]
        public int VerhuuraanvraagId { get; set; }
        public Verhuuraanvraag Verhuuraanvraag { get; set; }

        [Required]
        public int MedewerkerId { get; set; }
        public Medewerker Medewerker { get; set; }

        public List<Notitie>? Notities { get; set; }
        public List<Foto>? Fotos { get; set; }
    }
}
