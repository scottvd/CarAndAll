using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarAndAll.Server.Models
{
    public class Gebruiker : IdentityUser
    {
        [Required, MaxLength(255), EmailAddress]
        public string Email { get; set; }
    }
}
