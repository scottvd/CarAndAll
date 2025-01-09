using System.ComponentModel.DataAnnotations;

public class VerhuuraanvraagDTO
{
    [Required]
    public DateTime OphaalDatum { get; set; }

    [Required]
    public DateTime InleverDatum { get; set; }

    [Required]
    public int VoertuigId { get; set; }
}
