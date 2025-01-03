using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Bedrijf
    {
        [Key]
        public int KvkNummer { get; set; }

        [Required, MaxLength(255)]
        public string Naam { get; set; }

        [Required, MaxLength(255)]
        public string Adres { get; set; }

        [Required]
        public List<Huurder> Huurders { get; set; }

        public int? AbonnementId { get; set; }
        public Abonnement? Abonnement { get; set; }
    }
}