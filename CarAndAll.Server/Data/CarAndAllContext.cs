using CarAndAll.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarAndAll.Server.Data
{
    public class CarAndAllContext : IdentityDbContext<Gebruiker>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder b) => b.UseSqlite("Data Source=database.db");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Medewerker>().HasKey(m => m.PersoneelsNummer);
            modelBuilder.Entity<Medewerker>().HasIndex(m => m.PersoneelsNummer).IsUnique();
        }

        public DbSet<Abonnement> Abonnementen { get; set; }
        public DbSet<Bedrijf> Bedrijven { get; set; }
        public DbSet<Foto> Fotos { get; set; }
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Medewerker> Medewerkers { get; set; }
        public DbSet<Notitie> Notities { get; set; }
        public DbSet<Schademelding> Schademeldingen { get; set; }
        public DbSet<Verhuuraanvraag> Verhuuraanvragen { get; set; }
        public DbSet<Voertuig> Voertuigen { get; set; }
    }
}