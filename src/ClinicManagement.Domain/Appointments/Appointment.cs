using ClinicManagement.Domain.Common;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Doctors;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Patients;
using ClinicManagement.Domain.Sessions;

namespace ClinicManagement.Domain.Appointments
{
    public class Appointment : AuditableEntity
    {
        public Guid DoctorId { get; private set; }
        public Doctor? Doctor { get; private set; }

        public Guid PatientId { get; private set; }
        public Patient? Patient { get; private set; }

        public DateTime ScheduledAt { get; private set; }
        public TimeSpan Duration { get; private set; }

        public AppointmentStatus Status { get; private set; }

        public bool reminderSent { get; private set; } = false;

        public Session? Session { get; private set; }

        private Appointment() { }

        private Appointment(Guid id, Guid doctorId, Guid patientId,DateTime scheduledAt, TimeSpan duration)
            : base(id)
        {
            DoctorId = doctorId;
            PatientId = patientId;
            ScheduledAt = scheduledAt;
            Status = AppointmentStatus.Scheduled;
            Duration = duration;
        }

        public static Result<Appointment> Create(Guid id, Guid doctorId,  Guid patientId,DateTime scheduledAt, TimeSpan duration)
        {
            if (scheduledAt == default)
                return AppointmentErrors.ScheduledAtRequired;

            return new Appointment(id, doctorId, patientId,scheduledAt, duration);
        }

        public bool CanTransitionTo(AppointmentStatus newStatus)
        {
            return (Status, newStatus) switch
            {
                // Normal flow
                (AppointmentStatus.Scheduled, AppointmentStatus.Confirmed) => true,
                (AppointmentStatus.Confirmed, AppointmentStatus.Completed) => true,
                (AppointmentStatus.Confirmed, AppointmentStatus.Missed) => true,

                // Cancellation rules
                (AppointmentStatus.Scheduled, AppointmentStatus.Cancelled) => true,
                (AppointmentStatus.Confirmed, AppointmentStatus.Cancelled) => true,

                // No transitions allowed after completion
                (AppointmentStatus.Completed, _) => false,
                (AppointmentStatus.Missed, _) => false,
                (AppointmentStatus.Cancelled, _) => false,

                _ => false
            };
        }

        public Result<Updated> ChangeStatus(AppointmentStatus newStatus)
        {
            if (!CanTransitionTo(newStatus))
                return AppointmentErrors.InvalidAppointmentStatusTransition;

            Status = newStatus;
            return Result.Updated;
        }

        public DateTime PlannedEndTime => ScheduledAt.Add(Duration);

        public Result<Updated> Reschedule(DateTime newScheduledAt)
        {
            this.ScheduledAt = newScheduledAt;
            return Result.Updated;
        }

        public Result<Updated> SetReminderSend()
        {
            this.reminderSent = true;
            return Result.Updated;
        }



    }



}
