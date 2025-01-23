using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CarAndAll.Server.Models;
using CarAndAll.Services;
using CarAndAll.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace CarAndAll.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticatieController : ControllerBase
    {
        private readonly CarAndAllContext _context;

        private readonly SignInManager<Gebruiker> _signInManager;
        private readonly UserManager<Gebruiker> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IGebruikerRollenService _gebruikerRollenService;

        public AuthenticatieController(CarAndAllContext context, SignInManager<Gebruiker> signInManager, UserManager<Gebruiker> userManager, IConfiguration configuration, IGebruikerRollenService gebruikerRollenService)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _gebruikerRollenService = gebruikerRollenService;
        }

        [HttpPost("LogIn")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> LogIn([FromBody] LogInDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Foutmelding tijdens het inloggen: ", Errors = ModelState });
            }

            var gebruiker = await _userManager.FindByEmailAsync(dto.Email);

            if (gebruiker == null)
            {
                return Unauthorized(new { Message = "Geen gebruiker gevonden met deze gegevens." });
            }

            if (DatumService.IsVerlopen(gebruiker.WachtwoordBijgewerktDatum, 90))
            {
                return Ok(new { Verlopen = true });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(gebruiker, dto.Wachtwoord, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { Message = "Foutmelding tijdens het inloggen, probeer het later opnieuw" });
            }


            var rollen = await _userManager.GetRolesAsync(gebruiker);

            if (rollen == null || rollen.Count == 0)
            {
                return Unauthorized(new { Message = "Gebruiker heeft geen rollen" });
            }

            if(rollen.Any(r => new [] { "Particulier", "Zakelijk", "Wagenparkbeheerder" }.Contains(r) )) {
                var heeftVerwijderingsverzoek = await _context.Verwijderingsverzoeken.Where(v => v.HuurderId == gebruiker.Id).AnyAsync();

                if(heeftVerwijderingsverzoek) {
                    return Unauthorized( new { Message = "Verwijderde gebruikers kunnen niet inloggen" });
                }
            }

            var csrfToken = Guid.NewGuid().ToString();
            var token = JwtTokenService.GenereerJwtToken(gebruiker, rollen, _configuration);

            Response.Cookies.Append("csrfToken", csrfToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(6)
            });

            Response.Cookies.Append("jwtToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddHours(6)
            });

            return Ok(new { Message = "Inloggen geslaagd!" });
        }

        [HttpPost("LogUit")]
        public IActionResult LogUit()
        {
            Response.Cookies.Delete("csrfToken", new CookieOptions
            {
                HttpOnly = false,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            Response.Cookies.Delete("jwtToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok(new { Message = "Uitloggen geslaagd!" });
        }


        [HttpPut("ResetWachtwoord")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ResetWachtwoord([FromBody] ResetWachtwoordDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { Message = "Foutmelding tijdens het resetten van uw wachtwoord: ", Errors = ModelState });
            }

            var gebruiker = await _userManager.FindByEmailAsync(dto.Email);

            if (gebruiker == null)
            {
                return Unauthorized(new { Message = "Geen gebruiker gevonden met deze gegevens." });
            }

            if (dto.HuidigWachtwoord == dto.NieuwWachtwoord)
            {
                return Unauthorized(new { Message = "Het nieuwe wachtwoord mag niet gelijk zijn aan het oude wachtwoord!" });
            }

            var geldigHuidigWachtwoord = await _userManager.CheckPasswordAsync(gebruiker, dto.HuidigWachtwoord);
            
            if (!geldigHuidigWachtwoord)
            {
                return Unauthorized(new { Message = "Het huidige wachtwoord is niet correct." });
            }

            var result = await _userManager.ChangePasswordAsync(gebruiker, dto.HuidigWachtwoord, dto.NieuwWachtwoord);

            if (!result.Succeeded)
            {
                return BadRequest(new { Message = "Foutmelding tijdens het wijzigen van het wachtwoord.", Errors = result.Errors });
            }

            gebruiker.WachtwoordBijgewerktDatum = DateTime.UtcNow.Date;

            var updateResult = await _userManager.UpdateAsync(gebruiker);
            if (!updateResult.Succeeded)
            {
                return BadRequest(new { Message = "Foutmelding tijdens het opslaan van de wijziging van het wachtwoord." });
            }

            return Ok(new { Message = "Het wachtwoord is succesvol gewijzigd!" });
        }

        [HttpPost("HeeftToestemming")]
        public bool HeeftToestemming([FromBody] string[]? rollen)
        {
            var gebruikerRollen = _gebruikerRollenService.GetGebruikerRollen();

            if (gebruikerRollen != null)
            {
                if (rollen == null || !rollen.Any())
                {
                    return true;
                }

                try
                {
                    var heeftToestemming = rollen.Any(rol => gebruikerRollen.Contains(rol, StringComparer.OrdinalIgnoreCase));

                    return heeftToestemming;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return false;
        }
    }
}
