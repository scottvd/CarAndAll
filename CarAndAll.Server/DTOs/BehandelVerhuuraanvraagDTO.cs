using System.ComponentModel.DataAnnotations;
using CarAndAll.Server.Models;

public class BehandelVerhuuraanvraagDTO
{
    [Required]
    public int aanvraagID { get; set; }

    [Required]
    public string status { get; set; }
}
