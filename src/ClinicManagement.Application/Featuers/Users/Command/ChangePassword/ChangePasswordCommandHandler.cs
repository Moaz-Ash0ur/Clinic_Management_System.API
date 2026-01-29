using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Command.NewFolder___Copy
{
    public sealed class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<Success>>
    {
        private readonly IIdentityService _identityService;

        public ChangePasswordCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result<Success>> Handle(ChangePasswordCommand cmd, CancellationToken cancellationToken)
        {
            return await _identityService.ChangePasswordAsync(
                cmd.UserId,
                cmd.CurrentPassword,
                cmd.NewPassword);
        }



    }


}
