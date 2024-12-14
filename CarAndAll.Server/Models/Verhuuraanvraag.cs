using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public enum VerhuurStatus
    {
        InBehandeling,
        Geaccepteerd,
        Afgewezen
    }
    public class Verhuuraanvraag
    {
        [Key]
        public int AanvraagID { get; set; }

        [Required]
        public DateTime StartDatum { get; set; }

        [Required]
        public DateTime EindDatum { get; set; }

        [Required, MaxLength(50)]
        public VerhuurStatus Status { get; set; }

        [Required]
        public string KlantId { get; set; }
        public Klant Klant { get; set; }

        [Required]
        public int VoertuigId { get; set; }
        public Voertuig Voertuig { get; set; }

        public List<Schademelding>? Schademeldingen { get; set; } // Optioneel: Alleen bij schade
    }
}