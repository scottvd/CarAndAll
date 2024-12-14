using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CarAndAll.Server.Models;
using CarAndAll.Server.Data;

namespace CarAndAll.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BedrijfController : ControllerBase
    {
        private readonly UserManager<Gebruiker> _userManager;
        private readonly CarAndAllContext _dbContext;

        public BedrijfController(UserManager<Gebruiker> userManager, CarAndAllContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        // Endpoint om een zakelijke huurder toe te voegen
        [HttpPost("{bedrijfId}/voeg_zakelijke_huurder_toe")]
        public async Task<IActionResult> VoegZakelijkeHuurderToe(int bedrijfId, [FromBody] KlantDto klantDto)
        {
            // Controleer of het bedrijf bestaat
            var bedrijf = await _dbContext.Bedrijven.FindAsync(bedrijfId);
            if (bedrijf == null)
            {
                return NotFound("Bedrijf niet gevonden.");
            }

            // Zorg ervoor dat de ingelogde gebruiker een zakelijke beheerder is voor dit bedrijf
            var zakelijkeBeheerder = await _dbContext.Klanten
                .FirstOrDefaultAsync(k => k.Id == klantDto.GebruikerId && k.Type == KlantType.ZakelijkeBeheerder && k.BedrijfId == bedrijfId);

            if (zakelijkeBeheerder == null)
            {
                return BadRequest("Gebruiker is geen zakelijke beheerder voor dit bedrijf.");
            }

            // Maak een nieuwe zakelijke huurder aan
            var zakelijkeHuurder = new Klant
            {
                Naam = klantDto.Naam,
                Adres = klantDto.Adres,
                Email = klantDto.Email,
                Type = KlantType.ZakelijkeHuurder, // Stel het type in als ZakelijkeHuurder
                BedrijfId = bedrijfId // Koppel de zakelijke huurder aan het bedrijf
            };

            // Maak een account aan voor de zakelijke huurder
            var result = await _userManager.CreateAsync(zakelijkeHuurder, klantDto.Password);

            if (result.Succeeded)
            {
                // Voeg de nieuwe zakelijke huurder toe aan het bedrijf
                bedrijf.Klanten.Add(zakelijkeHuurder);
                await _dbContext.SaveChangesAsync();

                return Ok("Zakelijke huurder succesvol toegevoegd.");
            }

            return BadRequest(result.Errors);
        }
    }
}
