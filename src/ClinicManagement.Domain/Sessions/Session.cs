
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Patients.MedicalRecords;
using ClinicManagement.Domain.Prescriptions;
using ClinicManagement.Domain.Sessions.Attendances;

namespace ClinicManagement.Domain.Sessions
{

    public sealed class Session : AuditableEntity
    {
        public Guid AppointmentId { get; private set; }
        public Appointment? appointment { get; private set; }
        
        public DateTime? ActualStartTime { get; private set; }
        public DateTime? ActualEndTime { get; private set; }
        public SessionStatus Status { get; private set; }
        public string? DoctorNotes { get; private set; }

        public Attendance? attendance { get; private set; }
        public Prescription? Prescription { get; private set; }

        private readonly List<MedicalRecord> _medicalRecords = new();
        public IEnumerable<MedicalRecord> MedicalRecords => _medicalRecords.AsReadOnly();

        private Session() { }

        private Session(Guid id, Guid appointmentId) : base(id)
        {
            AppointmentId = appointmentId;
            Status = SessionStatus.Pending; 
        }

        public static Result<Session> Create(Guid id, Guid appointmentId)
        {
            if (id == Guid.Empty)
                return SessionErrors.EmptySessionId;

            if (appointmentId == Guid.Empty)
                return SessionErrors.AppointmentRequired;

            return new Session(id, appointmentId);
        } 


        public Result<Success> Start(DateTime actualStart)
        {
            if (Status != SessionStatus.Pending)
                return SessionErrors.InvalidSessionState;
     
            ActualStartTime = actualStart;
            Status = SessionStatus.InProgress;

            return Result.Success;
        }

        public Result<Success> Complete(DateTime actualEnd)
        {
            if (Status != SessionStatus.InProgress)
                return SessionErrors.InvalidSessionState;

            if (actualEnd < ActualStartTime)
                return SessionErrors.InvalidSessionTime;
      
            ActualEndTime = actualEnd;
            Status = SessionStatus.Completed;

            return Result.Success;
        }

        public Result<Success> MarkAsMissed()
        {
            //if (Status != SessionStatus.Pending)
            //    return SessionErrors.InvalidSessionState;

            //var appointmentResult = appointment.ChangeStatus(AppointmentStatus.Missed);
            //if (appointmentResult.IsError)
            //    return appointmentResult.Errors;

            //Status = SessionStatus.Missed;
            //attendance.MarkAbsent();

            return Result.Success;
        }

        public void AddDoctorNotes(string notes)
        {
            DoctorNotes = notes;
        }




    }

}
