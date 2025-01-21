using Microsoft.AspNetCore.Mvc;
using CarAndAll.Server.Data;
using CarAndAll.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CarAndAll.Services;

namespace CarAndAll.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VerhuuraanvraagController : ControllerBase
    {
        private readonly CarAndAllContext _context;
        private readonly IGebruikerIdService _gebruikerIdService;

        public VerhuuraanvraagController(CarAndAllContext context, IGebruikerIdService gebruikerIdService)
        {
            _context = context;
            _gebruikerIdService = gebruikerIdService;
        }

        [HttpGet("GetVoertuigen")]
        public async Task<IActionResult> GetVoertuigen([FromQuery] DateTime ophaalDatum, [FromQuery] DateTime inleverDatum)
        {
            if (ophaalDatum == DateTime.MinValue || inleverDatum == DateTime.MinValue)
            {
                return BadRequest("Vul een ophaal- en inleverdatum in.");
            }

            var resultaat = await _context.Voertuigen
                .Where(voertuig =>
                    !voertuig.Verhuuraanvragen.Any(v =>
                        (ophaalDatum >= v.OphaalDatum && ophaalDatum <= v.InleverDatum) ||
                        (inleverDatum >= v.OphaalDatum && inleverDatum <= v.InleverDatum) ||
                        (ophaalDatum <= v.OphaalDatum && inleverDatum >= v.InleverDatum)
                    )
                )
                .Where(voertuig =>
                    !_context.Schademeldingen.Any(schade =>
                        schade.VoertuigId == voertuig.VoertuigId &&
                        ophaalDatum <= schade.Datum.AddDays(schade.HerstelPeriode) &&
                        inleverDatum >= schade.Datum
                    )
                )
                .ToListAsync();

            return Ok(resultaat);
        }

        [HttpPost("AddVerhuuraanvraag")]
        [Authorize(Policy = "Huurders")]
        public async Task<IActionResult> AddVerhuuraanvraag([FromBody] VerhuuraanvraagDTO verhuuraanvraagDTO) 
        {
            if (verhuuraanvraagDTO.OphaalDatum == DateTime.MinValue || verhuuraanvraagDTO.InleverDatum == DateTime.MinValue) 
            {
                return BadRequest("Er is iets fout gegaan tijdens het maken van uw verhuuraanvraag. Probeer het opnieuw!");
            }

            var ophaalDatumUTC = verhuuraanvraagDTO.OphaalDatum.ToUniversalTime();
            var inleverDatumUTC = verhuuraanvraagDTO.InleverDatum.ToUniversalTime();

            var gewensteVoertuig = await _context.Voertuigen.FirstOrDefaultAsync(v => v.VoertuigId == verhuuraanvraagDTO.VoertuigId);

            if (gewensteVoertuig == null) 
            {
                return NotFound("Voertuig niet gevonden");
            }

            var gebruikerId = _gebruikerIdService.GetGebruikerId();

            if (gebruikerId == null || !await _context.Huurders.AnyAsync(g => g.Id == gebruikerId)) 
            {
                return Unauthorized("Er is geen ingelogde gebruiker gevonden, log in en probeer het opnieuw");
            }

            var overlappendeAanvragen = await _context.Verhuuraanvragen
                .Where(v => v.VoertuigId == verhuuraanvraagDTO.VoertuigId 
                            && (v.Status == AanvraagStatus.InBehandeling || v.Status == AanvraagStatus.Geaccepteerd)
                            && ophaalDatumUTC.Date < v.InleverDatum.Date && inleverDatumUTC.Date > v.OphaalDatum.Date) 
                .ToListAsync();

            if (overlappendeAanvragen.Any()) 
            {
                return BadRequest("Er is al een verhuuraanvraag voor dit voertuig binnen de opgegeven periode.");
            }

            var verhuuraanvraag = new Verhuuraanvraag 
            {
                OphaalDatum = ophaalDatumUTC,
                InleverDatum = inleverDatumUTC,
                Status = AanvraagStatus.InBehandeling,
                HuurderId = gebruikerId,
                VoertuigId = gewensteVoertuig.VoertuigId
            };

            try
            {
                _context.Verhuuraanvragen.Add(verhuuraanvraag);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Er is een probleem opgetreden bij het verwerken van uw aanvraag. Probeer het later opnieuw.");
            }

            return Ok("Verhuuraanvraag succesvol ingediend!");
        }


        [HttpGet("GetVerhuuraanvragen")]
        [Authorize(Policy = "BackofficeMedewerker")]
        public async Task<IActionResult> GetVerhuuraanvragen() 
        {
            var verhuuraanvragen = await _context.Verhuuraanvragen
                .Include(v => v.Huurder)
                .Include(v => v.Voertuig)
                .Select(v => new {
                    verhuuraanvraagID = v.VerhuuraanvraagId,
                    voertuig = $"{v.Voertuig.Merk} {v.Voertuig.Type}",
                    kenteken = v.Voertuig.Kenteken,
                    huurder = v.Huurder.Naam,
                    ophaaldatum = v.OphaalDatum.ToString("dd-MM-yyyy"),
                    inleverdatum = v.InleverDatum.ToString("dd-MM-yyyy"),
                    status = v.Status.ToString()
                })
                .ToListAsync();

            return Ok(verhuuraanvragen);
        }

        [HttpPost("BehandelVerhuuraanvraag")]
        [Authorize(Policy = "BackofficeMedewerker")]
        public async Task<IActionResult> BehandelVerhuuraanvraag([FromBody] BehandelVerhuuraanvraagDTO behandelVerhuuraanvraagDTO)
        {
            try
            {
                var aanvraag = await _context.Verhuuraanvragen
                    .Include(v => v.Voertuig)
                    .FirstAsync(v => v.VerhuuraanvraagId == behandelVerhuuraanvraagDTO.aanvraagID);

                if (aanvraag == null)
                {
                    return NotFound(new { message = "Verhuuraanvraag not found." });
                }

                switch (behandelVerhuuraanvraagDTO.status.ToLower())
                {
                    case "geaccepteerd":
                        aanvraag.Status = AanvraagStatus.Geaccepteerd;
                        break;

                    case "afgewezen":
                        aanvraag.Status = AanvraagStatus.Afgewezen;
                        break;

                    default:
                        return BadRequest(new { message = "Invalid status value." });
                }

                if (aanvraag.Status == AanvraagStatus.Geaccepteerd)
                {
                    var overlappendeAanvragen = await _context.Verhuuraanvragen
                        .Where(v =>
                            v.VoertuigId == aanvraag.VoertuigId &&
                            v.VerhuuraanvraagId != behandelVerhuuraanvraagDTO.aanvraagID &&
                            v.Status == AanvraagStatus.InBehandeling &&
                            (
                                (v.OphaalDatum >= aanvraag.OphaalDatum && v.OphaalDatum <= aanvraag.InleverDatum) ||
                                (v.InleverDatum >= aanvraag.OphaalDatum && v.InleverDatum <= aanvraag.InleverDatum) ||
                                (v.OphaalDatum <= aanvraag.OphaalDatum && v.InleverDatum >= aanvraag.InleverDatum)
                            ))
                        .ToListAsync();

                    foreach (var a in overlappendeAanvragen)
                    {
                        a.Status = AanvraagStatus.Afgewezen;
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Verhuuraanvraag succesvol behandeld." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", error = ex.Message });
            }
        }
    }
}
