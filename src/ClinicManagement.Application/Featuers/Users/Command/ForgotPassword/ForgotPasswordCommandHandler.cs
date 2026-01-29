using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Command.ForgotPassword
{
    public sealed class ForgotPasswordCommandHandler
        : IRequestHandler<ForgotPasswordCommand, Result<Success>>
    {
        private readonly IIdentityService _identityService;

        public ForgotPasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<Success>> Handle(
            ForgotPasswordCommand cmd,
            CancellationToken cancellationToken)
        {
            return await _identityService.SendResetPasswordLinkAsync(cmd.Email);
        }
    }

}
