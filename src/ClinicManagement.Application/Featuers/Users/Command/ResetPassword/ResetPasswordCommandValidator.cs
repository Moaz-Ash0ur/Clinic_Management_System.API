using FluentValidation;

namespace ClinicManagement.Application.Featuers.Users.Command.NewFolder
{
    public sealed class ResetPasswordCommandValidator
    : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Token)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(3);
                
        }
    }

}
