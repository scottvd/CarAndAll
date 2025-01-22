using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using CarAndAll.Server.Models;

namespace CarAndAll.Services
{
    public static class JwtTokenService
    {
        public static string GenereerJwtToken(Gebruiker gebruiker, IList<string> rollen, IConfiguration configuration)
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

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
