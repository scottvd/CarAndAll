using System.ComponentModel.DataAnnotations;

public class EditMedewerkerDTO
{
    [Required]
    public string MedewerkerID { get; set; }
    public string Email { get; set; }

    public string? OudWachtwoord { get; set; }
    public string? NieuwWachtwoord { get; set; }

    public string Naam { get; set; }

    public int PersoneelsNummer { get; set; }

    public bool Backoffice { get; set; }
}
