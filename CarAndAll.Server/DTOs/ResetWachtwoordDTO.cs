using System.ComponentModel.DataAnnotations;

public class ResetWachtwoordDTO
{
    [Required]
    public string Email { get; set; }
    
    [Required]
    public string HuidigWachtwoord { get; set; }

    [Required]
    public string NieuwWachtwoord { get; set; }
}