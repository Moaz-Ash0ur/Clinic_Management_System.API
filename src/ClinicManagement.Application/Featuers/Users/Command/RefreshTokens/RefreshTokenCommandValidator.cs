using ClinicManagement.Application.Featuers.Users.Command.RevokeRefreshToken;
using FluentValidation;


namespace ClinicManagement.Application.Featuers.Users.Command.RefreshTokens
{
    public sealed class RefreshTokenCommandValidator
    : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.")
                .MinimumLength(20).WithMessage("Refresh token is invalid.");

            RuleFor(x => x.IpAddress)
                .NotEmpty().WithMessage("IpAddress is required.")
                .ValidIp().WithMessage("IpAddress is not valid.");
        }

       
    }




}
