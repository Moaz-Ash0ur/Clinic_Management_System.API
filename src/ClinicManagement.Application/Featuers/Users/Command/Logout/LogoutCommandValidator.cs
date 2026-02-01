using ClinicManagement.Application.Featuers.Users.Command.RevokeRefreshToken;
using FluentValidation;

namespace ClinicManagement.Application.Featuers.Users.Command.Logout
{
    public sealed class LogoutCommandValidator
    : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator()
        {
            RuleFor(x => x.userId)
            .NotEmpty().WithMessage("UserId is required.")
            .Must(BeValidGuid).WithMessage("UserId must be a valid GUID.");

            RuleFor(x => x.IpAddress!)
                .NotEmpty().WithMessage("IpAddress is required.")
                .ValidIp().WithMessage("IpAddress is not valid.");
        }

     
        private bool BeValidGuid(string userId)
           => Guid.TryParse(userId, out _);
    }




}
