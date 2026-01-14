using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Domain.Sessions
{

    public static class SessionErrors
    {
        public static Error EmptySessionId =>
         Error.Validation("SessionId_Not_Allow_Null", "SessionId is required");

        public static Error AppointmentRequired =>
            Error.Validation("Appointment_Required", "Appointment is required for a session");

        public static Error InvalidSessionState =>
        Error.Validation(
            "Session.InvalidState",
            "Invalid session state transition");

        public static Error InvalidSessionTime =>
            Error.Validation(
                "Session.InvalidTime",
                "Session end time cannot be before start time");
    }





}
