using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Domain.Appointments
{
    public static class AppointmentErrors
    {
        public static Error ScheduledAtRequired =>
            Error.Validation("Appointment_ScheduledAt_Required", "Scheduled date and time is required");

        public static Error InvalidAppointmentStatusTransition =>
         Error.Validation(
        "Appointment.Status.InvalidTransition",
        "Invalid appointment status transition");



    }

}
