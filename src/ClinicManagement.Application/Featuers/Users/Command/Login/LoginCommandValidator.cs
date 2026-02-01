using FluentValidation;

namespace ClinicManagement.Application.Featuers.Users.Command.Login
{
    public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(request => request.Email)
                .NotNull().NotEmpty()
                .WithErrorCode("Email_Null_Or_Empty")
                .WithMessage("Email cannot be null or empty");

            RuleFor(request => request.Password)
                .NotNull().NotEmpty()
                .WithErrorCode("Password_Null_Or_Empty")
                .WithMessage("Password cannot be null or empty.");
        }
    }
}
