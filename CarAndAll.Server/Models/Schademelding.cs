namespace CarAndAll.Server.Models
{
    public class Schademelding
    {
        public DateTime Datum { get; set; }
        public required string Status { get; set; }
        public required string Beschrijving { get; set; }
        public Foto? Foto { get; set; }
    }
}