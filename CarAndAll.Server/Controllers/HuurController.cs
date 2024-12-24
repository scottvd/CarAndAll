using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using CarAndAll.Server.Data;
using CarAndAll.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CarAndAll.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HuurController : ControllerBase
    {
        private readonly CarAndAllContext _context;

        public HuurController(CarAndAllContext context)
        {
            _context = context;
        }

        [HttpGet("GetVoertuigen")]
        public async Task<IActionResult> GetVoertuigen([FromQuery] DateTime ophaalDatum, [FromQuery] DateTime inleverDatum) 
        {
            var resultaat = await _context.Voertuigen
                .Where(voertuig =>
                    voertuig.Verhuuraanvragen == null ||
                    !voertuig.Verhuuraanvragen.Any(v =>
                        (ophaalDatum >= v.StartDatum && ophaalDatum <= v.EindDatum) ||
                        (inleverDatum >= v.StartDatum && inleverDatum <= v.EindDatum) ||
                        (ophaalDatum <= v.StartDatum && inleverDatum >= v.EindDatum)
                    )
                )
                .ToListAsync();

            return Ok(resultaat);
        }
    }
}
