using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Featuers.Users.Queries.Login
{
    public class LoginHandler : IRequestHandler<LoginQuery, Result<AuthResponse>>
    {

        private readonly ILogger<LoginQuery> _logger;
        private readonly IIdentityService _identityService;
        private readonly ITokenProvider _tokenProvider;

        public LoginHandler(ILogger<LoginQuery> logger, IIdentityService identityService, ITokenProvider tokenProvider)
        {
            _logger = logger;
            _identityService = identityService;
            _tokenProvider = tokenProvider;
        }


        public async Task<Result<AuthResponse>> Handle(LoginQuery query, CancellationToken ct)
        {
            var userResponse = await _identityService.LoginAsync(query.Email, query.Password);

            if (userResponse.IsError)
            {
                return userResponse.Errors;
            }

            var generateTokenResult = await _tokenProvider.GenerateJwtTokenAsync(userResponse.Value, ct);

            if (generateTokenResult.IsError)
            {
                _logger.LogError("Generate token error occurred: {ErrorDescription}", generateTokenResult.TopError.Description);

                return generateTokenResult.Errors;
            }

            return generateTokenResult.Value;
        }
    }
}
