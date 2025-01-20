using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CarAndAll.Server.Data;
using CarAndAll.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CarAndAll.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoertuigBeheerController : ControllerBase
    {
        private readonly CarAndAllContext _context;

        public VoertuigBeheerController(CarAndAllContext context)
        {
            _context = context;
        }

        [HttpGet("GetAllVoertuigen")]
        public async Task<IActionResult> GetAllVoertuigen()
        {
            var voertuigen = await _context.Voertuigen.Where(v => v.IsActief == true).ToListAsync();
            return Ok(voertuigen);
        }

        [HttpGet("GetVoertuig/{id}")]
        public async Task<IActionResult> GetVoertuig(int Id)
        {
            var voertuig = await _context.Voertuigen.FindAsync(Id);
            if (voertuig == null)
            {
                return NotFound("Voertuig niet gevonden.");
            }
            return Ok(voertuig);
        }


        [HttpPost("AddVoertuig")]
        [Authorize(Policy = "Medewerkers")]
        public async Task<IActionResult> AddVoertuig([FromBody] Voertuig voertuig)
        {
            if (voertuig == null)
            {
                return BadRequest("Er is iets fout gegaan tijdens het toevoegen. Probeer het opnieuw!");
            }

            _context.Voertuigen.Add(voertuig);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetVoertuig), new { Id = voertuig.VoertuigId }, voertuig);
        }

        [HttpPut("EditVoertuig/{id}")]
        [Authorize(Policy = "Medewerkers")]
        public async Task<IActionResult> EditVoertuig(int Id, [FromBody] Voertuig bewerkingen)
        {
            if (bewerkingen == null || Id != bewerkingen.VoertuigId)
            {
                return BadRequest("Er is iets fout gegaan bij het bewerken van het voertuig. Probeer het opnieuw.");
            }

            var voertuig = await _context.Voertuigen.FindAsync(Id);

            if (voertuig == null)
            {
                return NotFound("Voertuig niet gevonden.");
            }

            if(bewerkingen.Kenteken != null) {
                voertuig.Kenteken = bewerkingen.Kenteken;
            }

            if(bewerkingen.Soort != null) {
                voertuig.Soort = bewerkingen.Soort;
            }

            if(bewerkingen.Merk != null) {
                voertuig.Merk = bewerkingen.Merk;
            }

            if(bewerkingen.Type != null) {
                voertuig.Type = bewerkingen.Type;
            }

            if(bewerkingen.Aanschafjaar != null) {
                voertuig.Aanschafjaar = bewerkingen.Aanschafjaar;
            }
            
            _context.Entry(voertuig).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(500, "Fout tijdens het bijwerken van het voertuig.");
            }

            return Ok("Voertuig succesvol bijgewerkt.");
        }

        [HttpPut("DeactiveerVoertuig/{id}")]
        [Authorize(Policy = "Medewerkers")]
        public async Task<IActionResult> DeactiveerVoertuig(int Id)
        {
            var voertuig = await _context.Voertuigen.FindAsync(Id);
            if (voertuig == null)
            {
                return NotFound("Voertuig niet gevonden.");
            }

            voertuig.IsActief = false;

            await _context.SaveChangesAsync();
            return Ok("Voertuig succesvol op non-actief gezet!");
        }
    }
}
