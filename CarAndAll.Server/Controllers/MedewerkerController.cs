using Microsoft.AspNetCore.Mvc;
using CarAndAll.Server.Data;
using CarAndAll.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CarAndAll.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MedewerkerController : ControllerBase
    {
        private readonly CarAndAllContext _context;
        private readonly UserManager<Gebruiker> _userManager;

        public MedewerkerController(CarAndAllContext context, UserManager<Gebruiker> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("GetMedewerkers")]
        [Authorize(Policy = "BackofficeMedewerker")]
        public async Task<IActionResult> GetMedewerkers()
        {
            var medewerkers = await _context.Medewerkers.ToListAsync();
            var medewerkersMetRol = new List<object>();

            foreach (var medewerker in medewerkers)
            {
                var rollen = await _userManager.GetRolesAsync(medewerker);
                var rol = rollen.Contains("BackofficeMedewerker") ? "BackofficeMedewerker" : "FrontofficeMedewerker";

                medewerkersMetRol.Add(new
                {
                    Id = medewerker.Id,
                    Naam = medewerker.Naam,
                    PersoneelsNummer = medewerker.PersoneelsNummer,
                    Email = medewerker.Email,
                    Rol = rol
                });
            }

            return Ok(medewerkersMetRol);
        }

        [HttpPost("AddMedewerker")]
        [Authorize(Policy = "BackofficeMedewerker")]
        public async Task<IActionResult> AddMedewerker([FromBody] MedewerkerDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Er is iets fout gegaan tijdens het toevoegen. Probeer het opnieuw!");
            }

            var bestaandMedewerker = await _context.Medewerkers.Where(m => m.NormalizedEmail.Equals(dto.Email.ToUpper()) || m.PersoneelsNummer == dto.Personeelsnummer).AnyAsync();

            if(bestaandMedewerker) {
                return Conflict("Er bestaat al een gebruiker met dit emailadres of personeelsnummer.");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try {
                var medewerker = new Medewerker
                    {
                        Email = dto.Email,
                        UserName = dto.Email,
                        Naam = dto.Naam,
                        PersoneelsNummer = dto.Personeelsnummer,
                    };

                var result = await _userManager.CreateAsync(medewerker, dto.Wachtwoord);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return BadRequest(new { Errors = errors });
                }
                
                await _userManager.AddToRoleAsync(medewerker, "FrontofficeMedewerker");

                await transaction.CommitAsync();
                return Ok("Medewerker succesvol toegevoegd");
            } catch (Exception ex) {
                await transaction.RollbackAsync();

                return StatusCode(500, new
                {
                    Message = "Er is iets fout gegaan.",
                    Error = ex.Message,
                });
            }
        }

        [HttpPut("EditMedewerker")]
        [Authorize(Policy = "BackofficeMedewerker")]
        public async Task<IActionResult> EditMedewerker([FromBody] EditMedewerkerDTO dto)
        {
            Console.WriteLine("medewerker: " + dto.MedewerkerID);
            if (!ModelState.IsValid)
            {
                return BadRequest("Er is iets fout gegaan tijdens het bewerken. Probeer het opnieuw!");
            }

            var medewerker = await _context.Medewerkers.FindAsync(dto.MedewerkerID);

            if (medewerker == null)
            {
                return BadRequest("Medewerker bestaat niet.");
            }

            if (dto.Naam != null &&!medewerker.Naam.Equals(dto.Naam))
            {
                medewerker.Naam = dto.Naam;
            }

            if (medewerker.PersoneelsNummer != dto.PersoneelsNummer)
            {
                medewerker.PersoneelsNummer = dto.PersoneelsNummer;
            }

            if (dto.Email != null && !medewerker.Email.Equals(dto.Email))
            {
                medewerker.Email = dto.Email;
            }

            if (!string.IsNullOrWhiteSpace(dto.NieuwWachtwoord))
            {
                var result = await _userManager.ChangePasswordAsync(medewerker, dto.OudWachtwoord, dto.NieuwWachtwoord);
                if (!result.Succeeded)
                {
                    return BadRequest("Wachtwoord kan niet worden gewijzigd.");
                }
            }

            if (dto.Backoffice)
            {
                if (!await _userManager.IsInRoleAsync(medewerker, "BackofficeMedewerker"))
                {
                    var addToRoleResult = await _userManager.AddToRoleAsync(medewerker, "BackofficeMedewerker");

                    if (!addToRoleResult.Succeeded)
                    {
                        return BadRequest("Kan de medewerker niet aan de BackofficeMedewerker rol toevoegen.");
                    }
                }
            }

            _context.Medewerkers.Update(medewerker);
            await _context.SaveChangesAsync();

            return Ok("Medewerker succesvol bijgewerkt.");
        }

        [HttpDelete("DeleteMedewerker")]
        [Authorize(Policy = "BackofficeMedewerker")]
        public async Task<IActionResult> DeleteMedewerker([FromBody] string Id)
        {
            var medewerker = await _context.Medewerkers.FindAsync(Id);
            if (medewerker == null)
            {
                return NotFound("Medewerker niet gevonden.");
            }

            _context.Medewerkers.Remove(medewerker);
            await _context.SaveChangesAsync();
            return Ok("Medewerker succesvol verwijderd!");
        }

    }
}
