using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public enum AanvraagStatus
    {
        InBehandeling,
        Geaccepteerd,
        Afgewezen,
        Afgerond
    }

    public class Verhuuraanvraag
    {
        [Key]
        public int AanvraagId { get; set; }

        [Required]
        public DateTime OphaalDatum { get; set; }

        [Required]
        public DateTime InleverDatum { get; set; }

        [Required, MaxLength(50)]
        public AanvraagStatus Status { get; set; }

        [Required]
        public string HuurderId { get; set; }
        public Huurder Huurder { get; set; }

        [Required]
        public int VoertuigId { get; set; }
        public Voertuig Voertuig { get; set; }

        public List<Schademelding>? Schademeldingen { get; set; }
    }
}