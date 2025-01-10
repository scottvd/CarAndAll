using System.ComponentModel.DataAnnotations;

public class MedewerkerDTO
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Wachtwoord { get; set; }

    [Required]
    public string Naam { get; set; }

    [Required]
    public int Personeelsnummer { get; set; }
}
