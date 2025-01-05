using System.ComponentModel.DataAnnotations;

public class LogInDTO
{
    [Required]
    public string Email { get; set; }

    [Required]
    public string Wachtwoord { get; set; }
}