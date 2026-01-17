using FluentValidation;

namespace ClinicManagement.Application.Featuers.MedicalRecords.Command.CreateMedicalRecord
{
    public sealed class CreateMedicalRecordCommandValidator
    : AbstractValidator<CreateMedicalRecordCommand>
    {
        public CreateMedicalRecordCommandValidator()
        {
            RuleFor(x => x.SessionId)
                .NotEmpty()
                .WithMessage("SessionId is required.");

            RuleFor(x => x.PatientId)
                .NotEmpty()
                .WithMessage("PatientId is required.");

            RuleFor(x => x.Diagnosis)
                .NotEmpty()
                .WithMessage("Diagnosis is required.")
                .MinimumLength(3)
                .WithMessage("Diagnosis must be at least 3 characters.")
                .MaximumLength(250)
                .WithMessage("Diagnosis must not exceed 250 characters.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000)
                .WithMessage("Notes must not exceed 1000 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Notes));
        }
    }




}
