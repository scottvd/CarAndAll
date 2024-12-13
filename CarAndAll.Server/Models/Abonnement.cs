using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public enum AbonnementType
    {
        PayAsYouGo,
        Prepaid
    }

    public class Abonnement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public AbonnementType Type { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Prijs moet positief zijn.")]
        public int Prijs { get; set; }
    }
}