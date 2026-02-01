using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Command.RefreshTokens
{
    public record RefreshTokenCommand(string RefreshToken, string IpAddress) : IRequest<Result<AuthResponse>>;




}
