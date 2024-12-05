    using System.ComponentModel.DataAnnotations;

    namespace CarAndAll.Server.Models
    {
        public class Notitie
        {
            [Key]
            public int NotitieID { get; set; }

            [Required, MaxLength(255)]
            public string Inhoud { get; set; }

            [Required]
            public Schademelding Schademelding { get; set; }

            [Required]
            public Medewerker Medewerker { get; set; }
        }
    }