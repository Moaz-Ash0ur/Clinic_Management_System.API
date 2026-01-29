using FluentValidation;

namespace ClinicManagement.Application.Featuers.Users.Command.NewFolder___Copy
{
    public sealed class ChangePasswordCommandValidator
     : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("Current password is required");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(3);

        }
    }


}
