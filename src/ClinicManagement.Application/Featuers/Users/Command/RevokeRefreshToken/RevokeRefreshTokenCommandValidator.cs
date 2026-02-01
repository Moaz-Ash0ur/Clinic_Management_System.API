using FluentValidation;

namespace ClinicManagement.Application.Featuers.Users.Command.RevokeRefreshToken
{
    public sealed class RevokeRefreshTokenCommandValidator
   : AbstractValidator<RevokeRefreshTokenCommand>
    {
        public RevokeRefreshTokenCommandValidator()
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
