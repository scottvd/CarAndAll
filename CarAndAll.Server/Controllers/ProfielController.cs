using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CarAndAll.Server.Models;
using CarAndAll.Server.Data;
using CarAndAll.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("GetGebruiker")]
        [Authorize(Policy = "Huurders")]
        public async Task<IActionResult> GetGebruiker() {
            var gebruikerId = _gebruikerIdService.GetGebruikerId();

            if (gebruikerId != null) {
                var gebruiker = await _context.Huurders
                    .Where(h => h.Id.Equals(gebruikerId))
                    .FirstOrDefaultAsync();

                if (gebruiker != null) {
                    var rollen = _gebruikerRollenService.GetGebruikerRollen();

                    if(rollen != null) {
                        string rol;

                        if (rollen.Contains("Wagenparkbeheerder")) {
                            rol = "Wagenparkbeheerder";
                        } else if (rollen.Contains("Zakelijk")) {
                            rol = "Zakelijk";
                        } else {
                            rol = "Particulier";
                        }

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

            return Unauthorized(new { Message = "U moet inloggen voor u uw profiel kunt inzien" });
        }

    }
}