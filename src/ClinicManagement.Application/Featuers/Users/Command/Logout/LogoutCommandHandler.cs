using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Users.Command.Logout;
using ClinicManagement.Domain.Common.Results;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Command.RevokeRefreshToken
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<Success>>
    {
        private readonly ITokenProvider _refreshTokenService;

        public LogoutCommandHandler(ITokenProvider refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Result<Success>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await _refreshTokenService.RevokeAllAsync(request.userId, request.IpAddress , cancellationToken);
        }
    }




}
