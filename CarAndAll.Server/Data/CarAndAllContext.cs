using CarAndAll.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarAndAll.Server.Data
{
    public class CarAndAllContext : IdentityDbContext<Gebruiker>
    {
        public CarAndAllContext(DbContextOptions<CarAndAllContext> options)
        : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Gebruiker>()
                .ToTable("Gebruikers");
            modelBuilder.Entity<Huurder>()
                .ToTable("Huurders"); 
            modelBuilder.Entity<Medewerker>()
                .ToTable("Medewerkers"); 

            modelBuilder.Entity<Huurder>()
                .HasOne(h => h.Bedrijf)
                .WithMany(b => b.Huurders)
                .HasForeignKey(h => h.BedrijfId);

            modelBuilder.Entity<Verhuuraanvraag>()
                .HasOne(v => v.Huurder)
                .WithMany(k => k.Verhuuraanvraagen)
                .HasForeignKey(v => v.HuurderId)
                .HasPrincipalKey(h => h.Id);

            modelBuilder.Entity<Verhuuraanvraag>()
                .HasOne(v => v.Voertuig)
                .WithMany(v => v.Verhuuraanvragen)
                .HasForeignKey(v => v.VoertuigId);

            modelBuilder.Entity<Voertuig>()
                .HasMany(v => v.Schademeldingen)
                .WithOne(s => s.Voertuig)
                .HasForeignKey(s => s.VoertuigId);

            modelBuilder.Entity<Schademelding>()
                .HasOne(s => s.Medewerker)
                .WithMany(m => m.Schademeldingen)
                .HasForeignKey(s => s.MedewerkerId);

            modelBuilder.Entity<Notitie>()
                .HasOne(n => n.Medewerker)
                .WithMany(m => m.Notities)
                .HasForeignKey(n => n.MedewerkerId);
        }

        public DbSet<Abonnement> Abonnementen { get; set; }
        public DbSet<Bedrijf> Bedrijven { get; set; }
        public DbSet<Foto> Fotos { get; set; }
        public DbSet<Huurder> Huurders { get; set; }
        public DbSet<Medewerker> Medewerkers { get; set; }
        public DbSet<Notitie> Notities { get; set; }
        public DbSet<Schademelding> Schademeldingen { get; set; }
        public DbSet<Verhuuraanvraag> Verhuuraanvragen { get; set; }
        public DbSet<Voertuig> Voertuigen { get; set; }
    }
}