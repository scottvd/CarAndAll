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
        public async Task<IActionResult> GetGeaccepteerdeVerhuuraanvragen([FromBody] DateTime datum) 
        {
            if (datum == DateTime.MinValue) 
            {
                return BadRequest("Er is geen datum bij het verzoek gevonden. Probeer het opnieuw.");
            }
            
            var resultaat = await _context.Verhuuraanvragen.Where(
                v => v.Status == AanvraagStatus.Geaccepteerd &&
                v.InleverDatum == datum
            ).ToListAsync();

            return Ok(resultaat);
        }

        [HttpPut("VerhuuraanvraagAfhandelen")]
        [Authorize(Policy = "Medewerkers")]
        public async Task<IActionResult> VerhuuraanvraagAfhandelen([FromBody] VerhuuraanvraagAfhandelenDTO dto) 
        {
            if(!ModelState.IsValid) {
                return BadRequest("Er is geen verhuuraanvraag bij uw opdracht meegegeven. Probeer het opnieuw.");
            }

            var verhuuraanvraag = await _context.Verhuuraanvragen.Where(
                v => v.AanvraagId == dto.VerhuuraanvraagId
            ).FirstAsync();

            if(verhuuraanvraag == null) {
                return NotFound("Er is geen verhuuraanvraag met het meegegeven ID gevonden.");
            }

            if(!dto.Beschrijving.IsNullOrEmpty() || dto.HerstelPeriode != 0) {
                if(dto.HerstelPeriode != 0 && !dto.Beschrijving.IsNullOrEmpty()) {
                    // Maak schademelding aan!!
                }

                return BadRequest("Bij het opstellen van een schadeformulier is het opgeven van een herstelperiode en beschrijving verplicht.");
            }

            return Ok();
        }
    }
}
