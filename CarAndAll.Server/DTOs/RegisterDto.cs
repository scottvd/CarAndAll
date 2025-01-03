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
    public HuurderType Type { get; set; }

    public int? BedrijfId { get; set; } // Alleen voor zakelijke klanten
}
