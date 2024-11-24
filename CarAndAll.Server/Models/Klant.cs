namespace CarAndAll.Server.Models
{

    public interface IKlant
    {
        string Naam { get; set; }
        string Adres { get; set; }
        string Email { get; set; }
    }

    public class Klant : IKlant
    {
        public required string Naam { get; set; }
        public required string Adres { get; set; }
        public required string Email { get; set; }
        // Met Get&Set neergezet, zodat het makkelijker is bij de UI gebruik.
    }

}