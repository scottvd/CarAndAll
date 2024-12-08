using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarAndAll.Server.Models
{
    public class Voertuig {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoertuigID { get; set; }
        
        [Required, MaxLength(255)]
        public string Kenteken { get; set; }

        [Required, MaxLength(255)]
        public string Soort { get; set; }

        [Required, MaxLength(255)]
        public string Merk { get; set; }

        [Required, MaxLength(255)]
        public string Type { get; set; }

        [Required]
        public int Aanschafjaar { get; set; }

        public List<Verhuuraanvraag>? Verhuuraanvragen { get; set; }

        public List<Schademelding>? Schademeldingen { get; set; }
    }
}