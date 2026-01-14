using FluentValidation;

namespace ClinicManagement.Application.Featuers.Appointments.Command.CreateAppointment
{
    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(x => x.DoctorId)
                .NotEmpty();

            RuleFor(x => x.PatientId)
                .NotEmpty();

            RuleFor(x => x.ScheduledAt)
                .Must(BeInFuture)
                .WithMessage("Appointment date must be in the future");
        }

        private bool BeInFuture(DateTime date)
        {
            return date > DateTime.UtcNow;
        }
    }


    //---------------------------------------------------------------------




}
