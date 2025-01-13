using System.IdentityModel.Tokens.Jwt;

namespace CarAndAll.Services
{
    public interface IGebruikerIdService
    {
        string GetGebruikerId();
    }

    public class GebruikerIdService : IGebruikerIdService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GebruikerIdService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetGebruikerId()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies["jwtToken"];
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        }
    }
}