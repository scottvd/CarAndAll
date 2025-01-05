    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using CarAndAll.Server.Models;
    using System.Collections.Generic;

    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticatieController : ControllerBase
    {
        private readonly SignInManager<Gebruiker> _signInManager;
        private readonly UserManager<Gebruiker> _userManager;
        private readonly IConfiguration _configuration;

        public AuthenticatieController(SignInManager<Gebruiker> signInManager, UserManager<Gebruiker> userManager, IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
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
            
            var csrfToken = Guid.NewGuid().ToString();
            var token = GenerateJwtToken(gebruiker, rollen);

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



        private string GenerateJwtToken(Gebruiker gebruiker, IList<string> rollen)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, gebruiker.Id),
                new Claim(JwtRegisteredClaimNames.Email, gebruiker.Email),
                new Claim(ClaimTypes.Name, gebruiker.UserName)
            };

            foreach (var rol in rollen)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
