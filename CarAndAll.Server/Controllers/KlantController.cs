using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CarAndAll.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CarAndAll.Server.Data;

namespace CarAndAll.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlantController : ControllerBase
    {
        private readonly UserManager<Gebruiker> _userManager;
        private readonly CarAndAllContext _dbContext;

        public KlantController(UserManager<Gebruiker> userManager, CarAndAllContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        // Endpoint voor klantregistratie
        [HttpPost("register_dto")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            // Maak de gebruiker aan als een Klant
            var klant = new Klant
            {
                UserName = model.Email,
                Naam = model.Naam,
                Adres = model.Adres,
                Email = model.Email,
                Type = model.Type, // Het type klant (Particulier of Zakelijk)
            };

            // Als het een zakelijke klant is, koppel het bedrijf
            if (model.Type == KlantType.ZakelijkeBeheerder && model.BedrijfId.HasValue)
            {
                var bedrijf = await _dbContext.Bedrijven.FindAsync(model.BedrijfId.Value);
                if (bedrijf == null)
                {
                    return BadRequest("Bedrijf niet gevonden.");
                }

                klant.Bedrijf = bedrijf;
            }

            // Probeer de gebruiker aan te maken in Identity
            var result = await _userManager.CreateAsync(klant, model.Password);

            if (result.Succeeded)
            {
                // Registratie is succesvol
                return Ok("Klant succesvol geregistreerd.");
            }

            // Fout bij registratie
            return BadRequest(result.Errors);
        }
    }
}
