using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CarAndAll.Server.Data;
using CarAndAll.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace CarAndAll.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HuurController : ControllerBase
    {
        private readonly CarAndAllContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HuurController(CarAndAllContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("GetVoertuigen")]
        public async Task<IActionResult> GetVoertuigen([FromQuery] DateTime ophaalDatum, [FromQuery] DateTime inleverDatum) 
        {
            if(ophaalDatum == DateTime.MinValue || inleverDatum == DateTime.MinValue) {
                return BadRequest("Vul een ophaal- en inleverdatum in.");
            }
            
            var resultaat = await _context.Voertuigen
                .Where(voertuig =>
                    voertuig.Verhuuraanvragen.Any() ||
                    !voertuig.Verhuuraanvragen.Any(v =>
                        (ophaalDatum >= v.OphaalDatum && ophaalDatum <= v.InleverDatum) ||
                        (inleverDatum >= v.OphaalDatum && inleverDatum <= v.InleverDatum) ||
                        (ophaalDatum <= v.OphaalDatum && inleverDatum >= v.InleverDatum)
                    )
                )
                .ToListAsync();

            return Ok(resultaat);
        }

        [HttpPost("DoeVerhuuraanvraag")]
        [Authorize(Policy = "Huurders")]
        public async Task<IActionResult> DoeVerhuuraanvraag([FromBody] VerhuuraanvraagDTO verhuuraanvraagDTO) 
        {
            if(verhuuraanvraagDTO.OphaalDatum == DateTime.MinValue || verhuuraanvraagDTO.InleverDatum == DateTime.MinValue) 
            {
                return BadRequest("Er is iets fout gegaan tijdens het maken van uw verhuuraanvraag. Probeer het opnieuw!");
            }
            
            var gewensteVoertuig = await _context.Voertuigen.FirstOrDefaultAsync(v => v.VoertuigID == verhuuraanvraagDTO.VoertuigId);
            
            if(gewensteVoertuig == null) 
            {
                return NotFound("Voertuig niet gevonden");
            }

            var isGehuurd = await _context.Verhuuraanvragen
                .AnyAsync(a => a.VoertuigId == verhuuraanvraagDTO.VoertuigId &&
                    a.Status != AanvraagStatus.Geaccepteerd &&
                    (
                        (a.OphaalDatum >= verhuuraanvraagDTO.OphaalDatum && a.OphaalDatum <= verhuuraanvraagDTO.InleverDatum) ||
                        (a.InleverDatum >= verhuuraanvraagDTO.OphaalDatum && a.InleverDatum <= verhuuraanvraagDTO.InleverDatum) ||
                        (a.OphaalDatum <= verhuuraanvraagDTO.OphaalDatum && a.InleverDatum >= verhuuraanvraagDTO.InleverDatum)
                    ));

            if(isGehuurd) 
            {
                return Conflict("Er bestaat al een verhuuraanvraag voor het geselecteerde voertuig binnen de gewenste periode");
            }

            var token = _httpContextAccessor.HttpContext.Request.Cookies["jwtToken"];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var gebruikerId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if(gebruikerId == null || !await _context.Huurders.AnyAsync(g => g.Id == gebruikerId)) 
            {
                return Unauthorized("Er is geen ingelogde gebruiker gevonden, log in en probeer het opnieuw");
            }

            Console.WriteLine("HuurderId:" + gebruikerId + " VoertuigId: " + verhuuraanvraagDTO.VoertuigId);

            var verhuuraanvraag = new Verhuuraanvraag 
            {
                OphaalDatum = verhuuraanvraagDTO.OphaalDatum,
                InleverDatum = verhuuraanvraagDTO.InleverDatum,
                Status = AanvraagStatus.InBehandeling,
                HuurderId = gebruikerId,
                VoertuigId = gewensteVoertuig.VoertuigID
            };

            try
            {
                _context.Verhuuraanvragen.Add(verhuuraanvraag);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Error saving verhuuraanvraag: {ex.Message}");
                return StatusCode(500, "Er is een probleem opgetreden bij het verwerken van uw aanvraag. Probeer het later opnieuw.");
            }

            return Ok("Verhuuraanvraag succesvol ingediend!");
        }
    }
}
