using ClinicManagement.Domain.Enums;
using FluentValidation;

namespace ClinicManagement.Application.Featuers.Users.Patients.Command.RegisterPatient
{
    public class RegisterPatientCommandValidator : AbstractValidator<RegisterPatientCommand>
    {
        public RegisterPatientCommandValidator()
        {
            RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name must be at most 50 characters");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name must be at most 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email address")
                .MaximumLength(100).WithMessage("Email must be at most 100 characters");

            RuleFor(x => x.password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");

            RuleFor(x => x.DateOfBirth.Date)
              .NotEmpty().WithMessage("Date of birth is required")
              .Must(BeAValidPastDate)
              .WithMessage("Date of birth must be in the past");


           RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required")
                .Must(g => g == Gender.Male || g == Gender.Female)
                .WithMessage("Gender must be either 'Male' or 'Female'");
        }

        private bool BeAValidPastDate(DateTime date)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var inputDate = DateOnly.FromDateTime(date);
            return inputDate < today; 
        }


    }

}
