using System.Security.Claims;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;


namespace ClinicManagement.Application.Common.Interfaces;

public interface ITokenProvider
{
    Task<Result<AuthResponse>> GenerateJwtTokenAsync(AppUserDto user, CancellationToken ct = default);

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);

    Task<Result<Success>> RevokeAsync(string token, CancellationToken ct);



}