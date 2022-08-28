using System.Security.Claims;
using PwCAssessment.Models;

namespace PwCAssessment.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    Task<UserRefreshToken?> UpdateUserRefreshTokenAsync(string tokenId, string newRefreshToken);
    Task<UserRefreshToken?> GetUserRefreshTokenAsync(string userId);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string? token);
    Task<UserRefreshToken?> GetUserRefreshTokenByEmailAsync(string? username);
    Task<UserRefreshToken?> CreateRefreshTokenAsync(UserRefreshToken userRefreshToken);
}