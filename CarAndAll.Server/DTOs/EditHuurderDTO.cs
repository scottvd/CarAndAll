using System.ComponentModel.DataAnnotations;

public class EditHuurderDTO
{
    [Required]
    public string HuurderID { get; set; }
    public string? Email { get; set; }
    public string? Naam { get; set; }
    public string? Adres { get; set; }
    public string? BedrijfsNaam { get; set; }
    public string? BedrijfsAdres { get; set; }
    public int? KVKNummer { get; set; }
    public string? OudWachtwoord { get; set; }
    public string? NieuwWachtwoord { get; set; }
    public string? Rol { get; set; }
}
