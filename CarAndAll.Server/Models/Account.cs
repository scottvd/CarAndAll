using System;

namespace CarAndAll.Server.Models
{
    public class Account
    {
        public required string Naam { get; set; }
        public required string Adres { get; set; }
        public required string Nummer { get; set; }
        public required string Email { get; set; }
    }
}