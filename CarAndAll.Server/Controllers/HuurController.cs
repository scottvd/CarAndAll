using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CarAndAll.Server.Data;
using CarAndAll.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CarAndAll.Services;
using Microsoft.IdentityModel.Tokens;

namespace CarAndAll.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HuurController : ControllerBase
    {
        private readonly CarAndAllContext _context;
        private readonly IGebruikerIdService _gebruikerIdService;

        public HuurController(CarAndAllContext context, IGebruikerIdService gebruikerIdService)
        {
            _context = context;
            _gebruikerIdService = gebruikerIdService;
        }

        [HttpGet("GetGeaccepteerdeVerhuuraanvragen")]
        [Authorize(Policy = "Medewerkers")]
        public async Task<IActionResult> GetGeaccepteerdeVerhuuraanvragen() 
        {
            var resultaat = await _context.Verhuuraanvragen
                .Where(
                    v => v.Status == AanvraagStatus.Geaccepteerd &&
                    v.InleverDatum <= DateTime.Today
                )
                .Select(v => new {
                    v.VerhuuraanvraagId,
                    v.InleverDatum,
                    Huurder = v.Huurder.Naam,
                    Voertuig = v.Voertuig.Merk + " " + v.Voertuig.Type
                })
                .ToListAsync();

            return Ok(resultaat);
        }

        [HttpPut("VerhuuraanvraagAfhandelen")]
        [Authorize(Policy = "Medewerkers")]
        public async Task<IActionResult> VerhuuraanvraagAfhandelen([FromBody] VerhuuraanvraagAfhandelenDTO dto) 
        {
            if(!ModelState.IsValid) {
                return BadRequest("Er is geen verhuuraanvraag bij uw opdracht meegegeven. Probeer het opnieuw.");
            }

            var medewerkerId = _gebruikerIdService.GetGebruikerId();

            if(medewerkerId.IsNullOrEmpty()) {
                return Unauthorized("Geen ingelogte medewerker gevonden. Log in en probeer het opnieuw!");
            }

            var verhuuraanvraag = await _context.Verhuuraanvragen.Where(
                v => v.VerhuuraanvraagId == dto.VerhuuraanvraagId
            ).FirstAsync();

            if(verhuuraanvraag == null) {
                return NotFound("Er is geen verhuuraanvraag met het meegegeven ID gevonden.");
            }

            verhuuraanvraag.Status = AanvraagStatus.Afgerond;

            var voertuig = await _context.Voertuigen.Where(v => v.VoertuigId == verhuuraanvraag.VoertuigId).FirstAsync();

            if(voertuig == null) {
                return NotFound("Er is geen voertuig bij de aanvraag gevonden.");
            }
        
            if (string.IsNullOrEmpty(dto.Beschrijving) ^ !dto.HerstelPeriode.HasValue)
            {
                return BadRequest("Bij het opstellen van een schadeformulier is het opgeven van een herstelperiode en beschrijving verplicht.");
            }

            if (!string.IsNullOrEmpty(dto.Beschrijving) && dto.HerstelPeriode.HasValue && dto.HerstelPeriode != 0)
            {
                var schademelding = new Schademelding
                {
                    Datum = DateTime.Today,
                    HerstelPeriode = (int)dto.HerstelPeriode,
                    Beschrijving = dto.Beschrijving,
                    VoertuigId = voertuig.VoertuigId,
                    VerhuuraanvraagId = verhuuraanvraag.VerhuuraanvraagId,
                    MedewerkerId = medewerkerId
                };

                _context.Schademeldingen.Add(schademelding);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Er is een probleem opgetreden bij het afhandelen van de verhuuraanvraag. Probeer het later opnieuw.");
            }

            return Ok();
        }
    }
}
