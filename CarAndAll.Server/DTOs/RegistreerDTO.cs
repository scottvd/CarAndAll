using System.ComponentModel.DataAnnotations;

public class RegistreerDTO
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Wachtwoord { get; set; }

    [Required]
    public string Naam { get; set; }

    [Required]
    public string Adres { get; set; }

    public bool Zakelijk { get; set; }
    public int? Kvk { get; set; }

    public string? BedrijfNaam { get; set; }
    public string? BedrijfAdres { get; set; }
}
