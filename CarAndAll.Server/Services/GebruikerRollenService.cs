using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CarAndAll.Services {
    public interface IGebruikerRollenService
    {
        List<string>? GetGebruikerRollen();
    }

    public class GebruikerRollenService : IGebruikerRollenService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GebruikerRollenService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public List<string>? GetGebruikerRollen()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwtToken"];

            if (string.IsNullOrEmpty(token)) {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            return jwtToken?.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
                .Select(c => c.Value)
                .ToList() ?? new List<string>();
        }
    }
}