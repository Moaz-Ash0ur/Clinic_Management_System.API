using FluentValidation;

namespace ClinicManagement.Application.Featuers.MedicalRecords.Command.UpdateMedicalRecord
{
    public sealed class UpdateMedicalRecordCommandValidator
    : AbstractValidator<UpdateMedicalRecordCommand>
    {
        public UpdateMedicalRecordCommandValidator()
        {
            RuleFor(x => x.MedicalRecordId)
                .NotEmpty()
                .WithMessage("MedicalRecordId is required.");

            RuleFor(x => x.Diagnosis)
                .NotEmpty()
                .WithMessage("Diagnosis is required.")
                .MinimumLength(3)
                .MaximumLength(500);

            RuleFor(x => x.Notes)
                .MaximumLength(300)
                .When(x => !string.IsNullOrWhiteSpace(x.Notes));
        }
    }


}
