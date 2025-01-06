using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

public class JwtAuthorizationHandler : AuthorizationHandler<RolRequirement>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolRequirement requirement)
    {
        var token = _httpContextAccessor.HttpContext.Request.Cookies["jwtToken"];

        if (string.IsNullOrEmpty(token))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        try
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            
            var rolClaims = jwtToken?.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
            Console.WriteLine("Roles in token: " + string.Join(", ", rolClaims)); 

            if (rolClaims != null && rolClaims.Any(role => requirement.Rollen.Contains(role)))
            {
                context.Succeed(requirement);
            }
            else
            {
                Console.WriteLine("User does not have any of the required roles.");
                context.Fail();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error while processing the JWT token: " + ex.Message);
            context.Fail();
        }

        return Task.CompletedTask;
    }
}

public class RolRequirement : IAuthorizationRequirement
{
    public IEnumerable<string> Rollen { get; }

    public RolRequirement(IEnumerable<string> rollen)
    {
        Rollen = rollen;
    }
}
