using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Identity;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ClinicManagement.Application.Featuers.Users.Command.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {

        private readonly ILogger<LoginCommand> _logger;
        private readonly IIdentityService _identityService;
        private readonly ITokenProvider _tokenProvider;
        private readonly IUnitOfWork _uow;
        private readonly IRepository<RefreshToken> _repo;

        public LoginCommandHandler(ILogger<LoginCommand> logger, IIdentityService identityService, ITokenProvider tokenProvider, IUnitOfWork uow)
        {
            _logger = logger;
            _identityService = identityService;
            _tokenProvider = tokenProvider;
            _uow = uow;
            _repo = _uow.GetRepository<RefreshToken>();
        }


        public async Task<Result<AuthResponse>> Handle(LoginCommand query, CancellationToken ct)
        {
            var userResponse = await _identityService.LoginAsync(query.Email, query.Password);

            if (userResponse.IsError)
            {
                return userResponse.Errors;
            }

            var generateTokenResult = await _tokenProvider.GenerateJwtTokenAsync(userResponse.Value, ct);


            var newRefreshTokenResult = RefreshToken.Create(
                Guid.NewGuid(),
                _tokenProvider.GenerateRefreshToken(),
                userResponse.Value.Id,
                DateTime.UtcNow.AddDays(7),
                query.ipAddress);

            if (newRefreshTokenResult.IsError)
                return newRefreshTokenResult.Errors;

            if (generateTokenResult.IsError)
            {
                _logger.LogError("Generate token error occurred: {ErrorDescription}", generateTokenResult.TopError.Description);

                return generateTokenResult.Errors;
            }

            var authResponse = generateTokenResult.Value;

            authResponse.RefreshToken =  newRefreshTokenResult.Value.Token;

             await _repo.AddAsync(newRefreshTokenResult.Value);
            await _uow.SaveChangesAsync();

            return authResponse;
        }
    }



 



}
