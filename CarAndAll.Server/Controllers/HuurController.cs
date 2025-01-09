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
                    voertuig.Verhuuraanvragen == null ||
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
        public async Task<IActionResult> DoeVerhuuraanvraag([FromBody] Voertuig voertuig, [FromBody] DateTime ophaalDatum, [FromBody] DateTime inleverDatum) {
            if(voertuig == null || ophaalDatum == DateTime.MinValue || inleverDatum == DateTime.MinValue) {
                return BadRequest("Er is iets fout gegaan tijdens het maken van uw verhuuraanvraag. Probeer het opnieuw!");
            }
            
            var gewensteVoertuig = _context.Voertuigen.FirstOrDefault(v => v.VoertuigID.Equals(voertuig.VoertuigID));
            
            if(gewensteVoertuig == null) {
                return NotFound("Voertuig niet gevonden");
            }

            var isBeschikbaar = await _context.Verhuuraanvragen
            .Where(a => a.VoertuigId == voertuig.VoertuigID && 
                        ((a.OphaalDatum >= ophaalDatum && a.OphaalDatum <= inleverDatum) ||
                        (a.InleverDatum >= ophaalDatum && a.InleverDatum <= inleverDatum) ||
                        (a.OphaalDatum <= ophaalDatum && a.InleverDatum >= inleverDatum)))
            .AnyAsync();

            if(!isBeschikbaar) {
                return Conflict("Er bestaat al een verhuuraanvraag voor het geselecteerde voertuig binnen de gewenste periode");
            }

            var token = _httpContextAccessor.HttpContext.Request.Cookies["jwtToken"];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var gebruikerId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "Sub")?.Value;

            if(gebruikerId == null) {
                return Unauthorized("Er is geen ingelogde gebruiker gevonden, log in en probeer het opnieuw");
            }

            var Verhuuraanvraag = new Verhuuraanvraag {
                OphaalDatum = ophaalDatum,
                InleverDatum = inleverDatum,
                Status = AanvraagStatus.InBehandeling,
                HuurderId = gebruikerId,
                VoertuigId = gewensteVoertuig.VoertuigID
            };

            return Ok();
        }
    }
}
