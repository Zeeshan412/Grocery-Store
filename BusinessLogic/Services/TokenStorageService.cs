using Microsoft.EntityFrameworkCore;
using WebApplication4.DataLayer.DbContext;
using WebApplication4.DTOs;

namespace WebApplication4.BusinessLogic.Services
{
    public class TokenStorageService : ITokenStorageService
    {
        private readonly ApplicationDbContext _context;

        public TokenStorageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveTokenAsync(string userId, string token, string jwtId, DateTime expiresAt)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = token,
                JwtId = jwtId,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt,
                IsRevoked = false,
                IsUsed = false
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<RefreshToken?> GetTokenByJwtIdAsync(string jwtId)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.JwtId == jwtId);
        }

        public async Task RevokeTokenAsync(string token)
        {
            var refreshToken = await GetTokenAsync(token);
            if (refreshToken != null)
            {
                refreshToken.IsRevoked = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task RevokeAllUserTokensAsync(string userId)
        {
            var tokens = await _context.RefreshTokens
                .Where(t => t.UserId == userId && !t.IsRevoked)
                .ToListAsync();

            foreach (var token in tokens)
            {
                token.IsRevoked = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsTokenValidAsync(string token)
        {
            var refreshToken = await GetTokenAsync(token);
            if (refreshToken == null)
                return false;

            if (refreshToken.IsRevoked || refreshToken.IsUsed)
                return false;

            if (refreshToken.ExpiresAt < DateTime.UtcNow)
                return false;

            return true;
        }
    }
}

