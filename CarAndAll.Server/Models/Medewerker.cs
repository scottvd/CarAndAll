    using System.ComponentModel.DataAnnotations;

    namespace CarAndAll.Server.Models
    {
        public class Medewerker
        {
            [Key]
            public int PersoneelsNummer { get; set; }

            [Required, MaxLength(255)]
            public string Naam { get; set; }

            [Required, MaxLength(255)]
            public string Adres { get; set; }

            [Required, MaxLength(255)]
            public string Email { get; set; }

            public List<Schademelding> Schademeldingen { get; set; }

            public List<Notitie> Notities { get; set; }

            public List<Foto> Fotos { get; set; }
        }
    }