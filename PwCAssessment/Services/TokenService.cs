using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PwCAssessment.Data;
using PwCAssessment.Interfaces;
using PwCAssessment.Models;

namespace PwCAssessment.Services;

    public class TokenService : ITokenService
    {
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _context;

    public TokenService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private string Issuer => _configuration["Issuer"];
        private string Audience => _configuration["Audience"];
        private string Secret => _configuration["Secret"];
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                   issuer: Issuer,
                   audience: Audience,
                   claims: claims,
                   notBefore: DateTime.UtcNow,
                   expires: DateTime.UtcNow.AddDays(7),
                   signingCredentials: signinCredentials
               );

            return  new JwtSecurityTokenHandler().WriteToken(tokeOptions); 
            
            //the method is called WriteToken but returns a string
        }


        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string? token)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = secretKey,
                ValidIssuer = Issuer,
                ValidAudience = Audience,
                ValidateLifetime = false //we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    
    

        public async Task<UserRefreshToken?> GetUserRefreshTokenAsync(string userId)
        {
            var user = await _context.UserRefreshTokens.SingleOrDefaultAsync(u => u.UserId == userId);

            return user;

        } 
        
        public async Task<UserRefreshToken?> CreateRefreshTokenAsync(UserRefreshToken? userRefreshToken)
        {
            if (userRefreshToken?.UserId == null || userRefreshToken.Username == null)
            {
                return null;

            }
            try
            {

                await  _context.UserRefreshTokens.AddAsync(userRefreshToken);
                await _context.SaveChangesAsync();

                return userRefreshToken;
            }
            catch
            {
                return null;
            }
           

        }
           public async Task<UserRefreshToken?> GetUserRefreshTokenByEmailAsync(string? username)
        {
            var user = await _context.UserRefreshTokens.SingleOrDefaultAsync(u => u.Username == username);

            return user;

        }

        public async Task<UserRefreshToken?> UpdateUserRefreshTokenAsync(string tokenId, string newRefreshToken)
        {
            var token = await _context.UserRefreshTokens.FirstOrDefaultAsync(x => x.Id == tokenId);
            if (token is null)
            {
                return null;
            }

        token.RefreshToken = newRefreshToken;
        _context.UserRefreshTokens.Update(token);
        await _context.SaveChangesAsync();

        return token;

        }
    }
