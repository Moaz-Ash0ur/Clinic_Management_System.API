using FluentValidation;

namespace ClinicManagement.Application.Featuers.Sessions.Queries.GetSessionById
{
    public sealed class GetSessionByIdQueryValidator
    : AbstractValidator<GetSessionByIdQuery>
    {
        public GetSessionByIdQueryValidator()
        {
            RuleFor(x => x.SessionId)
                .NotEmpty()
                .WithMessage("SessionId is required.");
        }
    }

}
