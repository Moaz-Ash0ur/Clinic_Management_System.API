using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Domain.DoctorWorkSchedules



{
    // DoctorWorkScheduleErrors.cs
    public static class DoctorWorkScheduleErrors
    {
        public static Error WorkDayInvalid =>
            Error.Validation("DoctorWorkSchedule_WorkDay_Invalid", "WorkDay must be between 0 and 6");

        public static Error InvalidTimeRange =>
            Error.Validation("DoctorWorkSchedule_TimeRange_Invalid", "StartTime must be before EndTime");

        public static Error OverlappingSchedule =>
           Error.Conflict("DoctorWorkSchedule_Overlapping_Schedule", "Overlapping Schedule Change another time");

        
    }

}
