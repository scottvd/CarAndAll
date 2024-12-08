using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.DTOs
{
    public class RegisterDto
    {
        [Required, MaxLength(255), EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required, MaxLength(255)]
        public string Naam { get; set; }

        [Required, MaxLength(255)]
        public string Adres { get; set; }

    }

}
