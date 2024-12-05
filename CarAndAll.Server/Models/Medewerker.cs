    using System.ComponentModel.DataAnnotations;

    namespace CarAndAll.Server.Models
    {
        public class Medewerker
        {
            [Key]
            public int PersoneelsNummer { get; set; }

            public List<Schademelding>? Schademeldingen { get; set; }

            public List<Notitie>? Notities { get; set; }

            public List<Foto>? Fotos { get; set; }
        }
    }