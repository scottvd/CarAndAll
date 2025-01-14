using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Verwijderingsverzoek
    {
        [Key]
        public int GegevensverwijderingId { get; set; }

        [Required]
        public DateTime Datum { get; set; }

        [Required]
        public string HuurderId { get; set; }
        public Huurder Huurder { get; set; }
    }
}