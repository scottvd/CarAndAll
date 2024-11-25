using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Abonnement
    {
        [Key]
        public string Type { get; set; }

        [Required]
        public int Prijs { get; set; }

        public List<Klant> Klanten { get; set; }      
    }
}