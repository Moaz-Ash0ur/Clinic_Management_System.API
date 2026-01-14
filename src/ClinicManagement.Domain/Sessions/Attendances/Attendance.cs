using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Patients;
using ClinicManagement.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Sessions.Attendances
{

    public sealed class Attendance : AuditableEntity
    {
        public Guid SessionId { get; private set; }
        public Session? Session { get; private set; }
        public Guid PatientId { get; private set; }
        public Patient? Patient { get; set; }

        public AttendanceStatus Status { get; private set; }
        public DateTime Date { get; private set; }


        private Attendance() { }

        private Attendance(Guid id, Guid sessionId, Guid PatientId, AttendanceStatus status, DateTime date): base(id)
        {
            SessionId = sessionId;
            Status = status;
            Date = date;
            this.PatientId = PatientId;
        }


        public static Result<Attendance> Create(Guid id, Guid sessionId,Guid PatientId, AttendanceStatus status, DateTime date)
        {
            if (id == Guid.Empty)
                return AttendanceErrors.AttendanceIdRequired;

            if (date == default)
                return AttendanceErrors.DateRequired;

            
            if (PatientId == Guid.Empty)
                return AttendanceErrors.PatientIdRequired;


            return new Attendance(id, sessionId, PatientId, status, date);
        }
      
        public Result<Updated> MarkPresent()
        {
            if (Status != AttendanceStatus.Pending)
                return AttendanceErrors.AttendanceAlreadyMarked;

            Status = AttendanceStatus.Present;
            return Result.Updated;
        }

        public Result<Updated> MarkAbsent()
        {
            if (Status != AttendanceStatus.Pending)
                return AttendanceErrors.AttendanceAlreadyMarked;

            Status = AttendanceStatus.Absent;
            return Result.Updated;
        }


    }

}
