using CarAndAll.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace YourNamespace
{
    public class CarAndAllContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder b) => b.UseSqlite("Data Source=database.db");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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