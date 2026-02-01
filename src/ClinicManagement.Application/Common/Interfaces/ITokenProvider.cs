using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Identity;
using System.Security.Claims;


namespace ClinicManagement.Application.Common.Interfaces;

public interface ITokenProvider
{
    Task<Result<AuthResponse>> GenerateJwtTokenAsync(AppUserDto user, CancellationToken ct = default);

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

    Task<Result<Success>> RevokeAsync(string token, string ipAddress, CancellationToken ct);

    Task<Result<Success>> RevokeAllAsync(string userId, string ipAddress, CancellationToken ct);   

    string GenerateRefreshToken();

    Task<Result<RefreshToken>> RotateAsync(RefreshToken currentToken, string ipAddress, CancellationToken ct = default);


}
