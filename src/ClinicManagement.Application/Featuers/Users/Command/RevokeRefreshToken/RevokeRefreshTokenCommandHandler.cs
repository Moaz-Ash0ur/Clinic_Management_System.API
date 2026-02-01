using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Command.RevokeRefreshToken
{
    public sealed class RevokeRefreshTokenCommandHandler : IRequestHandler<RevokeRefreshTokenCommand, Result<Success>>
    {
        private readonly ITokenProvider _refreshTokenService;

        public RevokeRefreshTokenCommandHandler(ITokenProvider refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        public async Task<Result<Success>> Handle(RevokeRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _refreshTokenService.RevokeAsync(request.RefreshToken, request.IpAddress, cancellationToken);
        }




    }




}
