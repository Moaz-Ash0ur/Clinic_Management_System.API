using FluentValidation;

namespace ClinicManagement.Application.Featuers.MedicalRecords.Queries.GetMedicalRecordById
{
    public sealed class GetMedicalRecordByIdQueryValidator
    : AbstractValidator<GetMedicalRecordByIdQuery>
    {
        public GetMedicalRecordByIdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("MedicalRecord Id is required.");
        }
    }


}
