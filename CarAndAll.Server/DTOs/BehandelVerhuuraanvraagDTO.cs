using System.ComponentModel.DataAnnotations;

public class BehandelVerhuuraanvraagDTO
{
    [Required]
    public int aanvraagID { get; set; }

    [Required]
    public string status { get; set; }
}
