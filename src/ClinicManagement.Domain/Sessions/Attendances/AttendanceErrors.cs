using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Domain.Sessions.Attendances
{
    // AttendanceErrors.cs
    public static class AttendanceErrors
    {
        public static Error DateRequired =>
            Error.Validation("Attendance_Date_Required", "Attendance date is required");

        public static Error AttendanceIdRequired =>
            Error.Validation("Attendance_ID_Required", "Attendance ID is required");

        public static Error PatientIdRequired =>
          Error.Validation("Patient_ID_Required", "Patient ID is required");

     
        public static Error AttendanceAlreadyMarked =>
       Error.Validation("Attendance.AlreadyMarked", "Attendance is already marked.");


    }

}
