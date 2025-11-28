using System.Security.Claims;

namespace WebApplication4.BusinessLogic.Services
{
    public interface IJwtTokenService
    {
        string GenerateToken(string userId, string email, string role);
        ClaimsPrincipal? ValidateToken(string token);
    }
}

