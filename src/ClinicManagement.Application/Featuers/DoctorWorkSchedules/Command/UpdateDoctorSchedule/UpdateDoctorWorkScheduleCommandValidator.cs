using FluentValidation;


namespace ClinicManagement.Application.Featuers.DoctorWorkSchedules.Command.UpdateDoctorSchedule
{
    public sealed class UpdateDoctorWorkScheduleCommandValidator : AbstractValidator<UpdateDoctorWorkScheduleCommand>
    {
        public UpdateDoctorWorkScheduleCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Schedule Id is required.");

            RuleFor(x => x.Day)
                .IsInEnum()
                .WithMessage("Invalid day of week.");

            RuleFor(x => x.StartTime)
                .NotEmpty()
                .WithMessage("Start time is required.");

            RuleFor(x => x.EndTime)
                .NotEmpty()
                .WithMessage("End time is required.");

            RuleFor(x => x)
                .Must(x => x.StartTime < x.EndTime)
                .WithMessage("Start time must be before end time.");

           

        }
    }



}
