using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Schademelding
    {
        [Key]
        public int SchademeldingID { get; set; }

        [Required, MaxLength(255)]
        public string Status { get; set; }

        [Required]
        public DateTime Datum { get; set; }

        [Required, MaxLength(255)]
        public string Beschrijving { get; set; }

        [Required]
        public Voertuig Voertuig { get; set; }

        [Required]
        public Verhuuraanvraag Verhuuraanvraag { get; set; }

        [Required]
        public Medewerker Medewerker { get; set; }

        public List<Notitie>? Notities { get; set; }

        public List <Foto>? Fotos { get; set; }
    }
}