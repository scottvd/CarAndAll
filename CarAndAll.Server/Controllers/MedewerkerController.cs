using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
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
            return Ok(medewerkers);
        }

        [HttpGet("GetMedewerker/{id}")]
        [Authorize(Policy = "BackofficeMedewerker")]
        public async Task<IActionResult> GetMedewerker(string Id)
        {
            var medewerker = await _context.Medewerkers.FindAsync(Id);
            if (medewerker == null)
            {
                return NotFound("Medewerker niet gevonden.");
            }
            return Ok(medewerker);
        }


        [HttpPost("AddMedewerker")]
        [Authorize(Policy = "BackofficeMedewerker")]
        public async Task<IActionResult> AddMedewerker([FromBody] MedewerkerDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Er is iets fout gegaan tijdens het toevoegen. Probeer het opnieuw!");
            }

            var gebruiktEmail = await _context.Medewerkers.Where(m => m.NormalizedEmail.Equals(dto.Email.ToUpper())).AnyAsync();

            if(gebruiktEmail) {
                return Conflict("Er bestaat al een gebruiker met dit emailadres.");
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

        [HttpPut("EditMedewerker/{id}")]
        [Authorize(Policy = "BackofficeMedewerkers")]
        public async Task<IActionResult> EditMedewerker()
        {
            return Ok("Medewerker succesvol bijgewerkt.");
        }
    }
}
