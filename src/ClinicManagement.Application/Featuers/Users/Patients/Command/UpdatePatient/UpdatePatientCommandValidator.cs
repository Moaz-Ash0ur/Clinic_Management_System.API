using FluentValidation;

namespace ClinicManagement.Application.Featuers.Users.Patients.Command.UpdatePatient
{
    public class UpdatePatientCommandValidator : AbstractValidator<UpdatePatientCommand>
    {
        public UpdatePatientCommandValidator()
        {
            RuleFor(x => x.Id)
               .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email  is required")
                .EmailAddress()
                .WithMessage("Invalid email");
              
        }
    }

}
