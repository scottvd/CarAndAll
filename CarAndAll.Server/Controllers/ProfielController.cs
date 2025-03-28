using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CarAndAll.Server.Models;
using CarAndAll.Server.Data;
using CarAndAll.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace CarAndAll.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfielController : ControllerBase
    {
        private readonly CarAndAllContext _context;
        private readonly UserManager<Gebruiker> _userManager;
        private readonly IGebruikerIdService _gebruikerIdService;
        private readonly IGebruikerRollenService _gebruikerRollenService;


        public ProfielController(CarAndAllContext context, UserManager<Gebruiker> userManager, IGebruikerIdService gebruikerIdService, IGebruikerRollenService gebruikerRollenService)
        {
            _context = context;
            _userManager = userManager;
            _gebruikerIdService = gebruikerIdService;
            _gebruikerRollenService = gebruikerRollenService;
        }

        [HttpGet("GetHuurder")]
        [Authorize(Policy = "Huurders")]
        public async Task<IActionResult> GetHuurder() {
            var gebruikerId = _gebruikerIdService.GetGebruikerId();

            if (gebruikerId != null) {
                var gebruiker = await _context.Huurders
                    .Where(h => h.Id.Equals(gebruikerId))
                    .FirstOrDefaultAsync();

                if (gebruiker != null) {
                    var rollen = _gebruikerRollenService.GetGebruikerRollen();

                    if(rollen != null) {
                        string rol = "Particulier";

                        if (rollen.Contains("Wagenparkbeheerder")) {
                            rol = "Wagenparkbeheerder";

                            var bedrijf = await _context.Bedrijven
                                .Where(b => b.Huurders.Any(h => h.Id == gebruikerId))
                                .FirstAsync();

                            if (bedrijf != null) {
                                var wagenparkbeheerder = new {
                                    gebruiker.Id,
                                    gebruiker.Naam,
                                    gebruiker.Email,
                                    gebruiker.Adres,
                                    bedrijfsNaam = bedrijf.Naam,
                                    bedrijfsAdres = bedrijf.Adres,
                                    bedrijf.KVKNummer,
                                    Rol = rol
                                };

                                return Ok(wagenparkbeheerder);
                            }
                        } 
                        else if (rollen.Contains("Zakelijk")) {
                            rol = "Zakelijk";

                            var zakelijkHuurder = new {
                                gebruiker.Id,
                                gebruiker.Naam,
                                gebruiker.Email,
                                Rol = rol
                            };

                            return Ok(zakelijkHuurder);
                        } 
                        else {
                            var gebruikerMetRol = new {
                                gebruiker.Id,
                                gebruiker.Naam,
                                gebruiker.Email,
                                gebruiker.Adres,
                                Rol = rol
                            };

                            return Ok(gebruikerMetRol);
                        }
                    }                    
                }
            }

            return Unauthorized(new { Message = "U moet inloggen voor u uw profiel kunt inzien" });
        }

        [HttpPut("EditHuurder")]
        [Authorize(Policy = "Huurders")]
        public async Task<IActionResult> EditHuurder([FromBody] EditHuurderDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Er is iets fout gegaan tijdens het bewerken. Probeer het opnieuw!");
            }

            var huurder = await _context.Huurders.FindAsync(dto.HuurderID);

            if (huurder == null)
            {
                return BadRequest("Huurder bestaat niet.");
            }

            if (dto.Naam != null && !huurder.Naam.Equals(dto.Naam))
            {
                huurder.Naam = dto.Naam;
            }

            if(dto.Rol != null) {
                if(dto.Rol.Equals("Particulier") && dto.Adres != null && !huurder.Adres.Equals(dto.Adres)) {
                    huurder.Adres = dto.Adres;
                }

                if(dto.Rol.Equals("Wagenparkbeheerder")) {
                    var bedrijf = await _context.Bedrijven
                        .Where(b => b.Huurders.Any(h => h.Id == dto.HuurderID))
                        .FirstAsync();

                    if(bedrijf != null) {
                        if(dto.BedrijfsAdres != null && !bedrijf.Adres.Equals(dto.BedrijfsAdres)) {
                            bedrijf.Adres = dto.BedrijfsAdres;
                        }

                        if(dto.BedrijfsNaam != null && !bedrijf.Naam.Equals(dto.BedrijfsNaam)) {
                            bedrijf.Naam = dto.BedrijfsNaam;
                        }

                        if(dto.KVKNummer != null && bedrijf.KVKNummer != dto.KVKNummer) {
                            bedrijf.KVKNummer = (int)dto.KVKNummer;
                        }
                    }
                }
            }

            if (dto.Email != null && !huurder.Email.Equals(dto.Email))
            {
                huurder.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.NieuwWachtwoord))
            {
                var result = await _userManager.ChangePasswordAsync(huurder, dto.OudWachtwoord, dto.NieuwWachtwoord);
                if (!result.Succeeded)
                {
                    return BadRequest("Wachtwoord kan niet worden gewijzigd.");
                }
            }

            _context.Huurders.Update(huurder);
            await _context.SaveChangesAsync();

            return Ok("Medewerker succesvol bijgewerkt.");
        }

        [HttpPost("AddVerwijderingsverzoek")]
        [Authorize(Policy = "Huurders")]
        public async Task<IActionResult> AddVerwijderingsverzoek([FromBody] string HuurderId)
        {
            if (HuurderId.IsNullOrEmpty())
            {
                return BadRequest("Geen gebruiker gevonden. Log in en probeer het opnieuw!");
            }

            var huurder = await _context.Huurders.Where(h => h.Id.Equals(HuurderId)).FirstAsync();

            if(huurder == null) {
                return BadRequest("Geen gebruiker gevonden. Log in en probeer het opnieuw!");
            }

            try {
                var verwijderingsverzoek = new Verwijderingsverzoek {
                    Datum = DateTime.UtcNow.Date,
                    Huurder = huurder
                };

                _context.Verwijderingsverzoeken.Add(verwijderingsverzoek);
                await _context.SaveChangesAsync();
                
                return Ok("Verwijderingsverzoek succesvol ingediend, over 6 maanden worden uw gegevens definitief verwijderd.");
            } catch (Exception ex) {
                return StatusCode(500, new
                {
                    Message = "Er is iets fout gegaan.",
                    Error = ex.Message,
                });
            }
        }
    }
}
