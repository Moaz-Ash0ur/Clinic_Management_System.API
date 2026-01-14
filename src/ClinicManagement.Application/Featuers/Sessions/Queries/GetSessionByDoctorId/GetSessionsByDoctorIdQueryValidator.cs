using FluentValidation;

namespace ClinicManagement.Application.Featuers.Sessions.Queries.GetSessionByDoctorId
{
    public sealed class GetSessionsByDoctorIdQueryValidator
    : AbstractValidator<GetSessionsByDoctorIdQuery>
    {
        public GetSessionsByDoctorIdQueryValidator()
        {
            RuleFor(x => x.DoctorId)
                .NotEmpty()
                .WithMessage("DoctorId is required.");
        }
    }


}
