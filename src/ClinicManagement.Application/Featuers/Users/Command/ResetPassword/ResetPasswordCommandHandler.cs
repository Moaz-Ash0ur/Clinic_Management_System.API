using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Command.NewFolder
{
    public sealed class ResetPasswordCommandHandler
        : IRequestHandler<ResetPasswordCommand, Result<Success>>
    {
        private readonly IIdentityService _identityService;

        public ResetPasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<Success>> Handle(
            ResetPasswordCommand cmd,
            CancellationToken cancellationToken)
        {
            return await _identityService.ResetPasswordAsync(
                cmd.Email,
                cmd.Token,
                cmd.NewPassword);
        }
    }

}
