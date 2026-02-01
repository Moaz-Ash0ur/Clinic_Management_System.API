using ClinicManagement.Domain.Common.Results;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Command.RevokeRefreshToken
{

    public record RevokeRefreshTokenCommand(string RefreshToken, string IpAddress) : IRequest<Result<Success>>;




}
