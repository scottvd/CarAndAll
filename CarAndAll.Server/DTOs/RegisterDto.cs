using CarAndAll.Server.Models;
using System.ComponentModel.DataAnnotations;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }

    [Required]
    public string Naam { get; set; }

    [Required]
    public string Adres { get; set; }

    [Required]
    public KlantType Type { get; set; } // Het type klant: Particulier of Zakelijk

    public int? BedrijfId { get; set; } // Alleen voor zakelijke klanten
}
