using WebApplication4.DTOs;

namespace WebApplication4.BusinessLogic.Services
{
    public interface ITokenStorageService
    {
        Task SaveTokenAsync(string userId, string token, string jwtId, DateTime expiresAt);
        Task<RefreshToken?> GetTokenAsync(string token);
        Task<RefreshToken?> GetTokenByJwtIdAsync(string jwtId);
        Task RevokeTokenAsync(string token);
        Task RevokeAllUserTokensAsync(string userId);
        Task<bool> IsTokenValidAsync(string token);
    }
}

