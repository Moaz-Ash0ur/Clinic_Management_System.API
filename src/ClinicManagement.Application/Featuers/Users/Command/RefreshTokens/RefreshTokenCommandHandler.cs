using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Identity;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Command.RefreshTokens
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
    {

        private readonly ITokenProvider _tokenService;
        private readonly IUnitOfWork _uow;
        private readonly IRepository<RefreshToken> _repo;
        private readonly IUserService _userService;
        public RefreshTokenCommandHandler(ITokenProvider tokenService, IUnitOfWork uow, IUserService userService)
        {
            _tokenService = tokenService;
            _uow = uow;
            _repo = _uow.GetRepository<RefreshToken>();
            _userService = userService;
        }


        public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand cmd, CancellationToken ct)
        {

            var refreshToken = await _repo.FindAsync(t => t.Token == cmd.RefreshToken);
            if (refreshToken == null)
                return ApplicationErrors.RefreshTokenNotFound;

            if (!refreshToken.IsActive)
                return ApplicationErrors.RefreshTokenExpired;

           
            var getUserResult = await _userService.GetUserByIdAsync(refreshToken.UserId);
            if (getUserResult.Value == null)
                return ApplicationErrors.UnauthorizedAction;

            var user = getUserResult.Value;

            var newRefreshToken = await _tokenService.RotateAsync(refreshToken, cmd.IpAddress, ct);

            if (newRefreshToken.IsError)
                return newRefreshToken.Errors;
            
            var accessTokenResult = await _tokenService.GenerateJwtTokenAsync(user, ct);

            if (accessTokenResult.IsError)
                return accessTokenResult.Errors;


            var authResponse = accessTokenResult.Value;

            authResponse.RefreshToken = newRefreshToken.Value.Token;

            return authResponse;
        }



    }




}
