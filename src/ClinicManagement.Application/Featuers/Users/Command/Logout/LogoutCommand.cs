using ClinicManagement.Domain.Common.Results;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Command.Logout
{
    public record LogoutCommand(string userId, string IpAddress) : IRequest<Result<Success>>;




}
