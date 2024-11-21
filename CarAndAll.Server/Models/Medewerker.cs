using System;

namespace CarAndAll.Server.Models
{
    interface IMedewerker
    {
        int Personeelsnummer { get; set; }
        Account Account { get; set; }
    }

    public class Medewerker : IMedewerker
    {
        public required int Personeelsnummer { get; set; }
        public required Account Account { get; set; }
    }
}
