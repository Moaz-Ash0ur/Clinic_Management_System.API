using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Identity;
using ClinicManagement.Infrastructure.Repsitories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ClinicManagement.Infrastructure.Identity
{
    public class TokenProvider : ITokenProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _uow;
        private IRepository<RefreshToken> _refreshTokenRepo;

        public TokenProvider(IConfiguration configuration, IUnitOfWork uow)
        {
            _configuration = configuration;
            _uow = uow;
            _refreshTokenRepo = _uow.GetRepository<RefreshToken>();

        }


        public async Task<Result<AuthResponse>> GenerateJwtTokenAsync(AppUserDto user, CancellationToken ct = default)
        {
            var tokenResult = await CreateAsync(user, ct);

            if (tokenResult.IsError)
            {
                return tokenResult.Errors;
            }

            return tokenResult.Value;
        } 

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]!)),
                ValidateIssuer = true,
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["JwtSettings:Audience"],
                ValidateLifetime = false, // Ignore token expiration
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token.");
            }

            return principal;
        }


        private async Task<Result<AuthResponse>> CreateAsync(AppUserDto user, CancellationToken ct = default)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            var issuer = jwtSettings["Issuer"]!;
            var audience = jwtSettings["Audience"]!;
            var key = jwtSettings["Secret"]!;

            var expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["TokenExpirationInMinutes"]!));

            var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id!),
            new (JwtRegisteredClaimNames.Email, user.Email!),
            new(ClaimTypes.Role, user.role.ToString())
        };

                      
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(descriptor);          

            return new AuthResponse
            {
                AccessToken = tokenHandler.WriteToken(securityToken),
                ExpiresOnUtc = expires
            };
        }

        public string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }


        public async Task<Result<Success>> RevokeAsync(string token, string ipAddress, CancellationToken ct)
        {

            var refreshToken = await _refreshTokenRepo.FindAsync(rf => rf.Token == token);
            if (refreshToken is null)
                return RefreshTokenErrors.NotFound;


            var revokeResult = refreshToken.Revoke(ipAddress);
            if (revokeResult.IsError)
                return revokeResult.Errors;

            await _uow.SaveChangesAsync();
            return Result.Success;
        }

        public async Task<Result<RefreshToken>> RotateAsync(RefreshToken currentToken,string ipAddress,CancellationToken ct = default)
        {
            var revokeResult = currentToken.Revoke(ipAddress,currentToken.Id);
            if (revokeResult.IsError)
               return RefreshTokenErrors.AlreadyRevoked;


            var newRefreshTokenResult = RefreshToken.Create(
                Guid.NewGuid(),
                GenerateRefreshToken(),
                currentToken.UserId,
                DateTime.UtcNow.AddDays(7),
                ipAddress);

            if (newRefreshTokenResult.IsError)
                return newRefreshTokenResult.Errors;


            var newRefreshToken = newRefreshTokenResult.Value;

            await _refreshTokenRepo.AddAsync(newRefreshToken);
            await _uow.SaveChangesAsync();

            return newRefreshToken;
        }

        public async Task<Result<Success>> RevokeAllAsync(string userId, string ipAddress, CancellationToken ct)
        {
            var activeRefreshToken = await  _refreshTokenRepo
                .GetQueryable()
                .Where(t => t.UserId == userId)
                .ToListAsync();

            if(activeRefreshToken == null)
                return RefreshTokenErrors.NotFound;


            foreach (var refToken in activeRefreshToken)
            {
                refToken.Revoke(ipAddress);
            }

            return Result.Success;
        }



    }
}
