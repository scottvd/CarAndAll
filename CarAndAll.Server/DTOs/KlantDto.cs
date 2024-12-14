using System.ComponentModel.DataAnnotations;

public class KlantDto
{
    [Required]
    public string Naam { get; set; }

    [Required]
    public string Adres { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }

    public string GebruikerId { get; set; } // Dit kan de ID zijn van de zakelijke beheerder
}
