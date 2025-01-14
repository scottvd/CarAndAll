    using System.ComponentModel.DataAnnotations;

    namespace CarAndAll.Server.Models
    {
        public class Foto
        {
            [Key]
            public int FotoID { get; set; }

            [Required, MaxLength(255)]
            public string Path { get; set; }

            [Required]
            public Schademelding Schademelding { get; set; }

            [Required]
            public string MedewerkerId { get; set; }
            public Medewerker Medewerker { get; set; } 
        }
    }