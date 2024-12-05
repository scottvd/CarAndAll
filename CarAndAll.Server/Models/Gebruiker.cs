using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace CarAndAll.Server.Models
    {
        public class Gebruiker : IdentityUser
        {

            [Required, MaxLength(255)]
            public string Naam { get; set; }

            [Required, MaxLength(255)]
            public string Adres { get; set; }

            [Required, MaxLength(255)]
            public string Email { get; set; }
        }
    }