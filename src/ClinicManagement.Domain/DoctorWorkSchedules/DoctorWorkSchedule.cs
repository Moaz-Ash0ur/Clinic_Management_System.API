using ClinicManagement.Domain.Common;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Doctors;
using ClinicManagement.Domain.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.DoctorWorkSchedules
{

    public sealed class DoctorWorkSchedule : AuditableEntity
    {
        public Guid DoctorId { get; private set; }
        public Doctor? Doctor { get; private set; }

        public WorkDay dayofWeek { get; private set; } 
        public TimeOnly StartTime { get; private set; }
        public TimeOnly EndTime { get; private set; }

        private DoctorWorkSchedule() { }

        private DoctorWorkSchedule(Guid id, Guid doctorId, Doctor doctor, WorkDay day,
            TimeOnly startTime, TimeOnly endTime)
            : base(id)
        {
            DoctorId = doctorId;
            Doctor = doctor;
            dayofWeek = day;
            StartTime = startTime;
            EndTime = endTime;
        }

        public static Result<DoctorWorkSchedule> Create(Guid id, Guid doctorId, Doctor doctor,
            WorkDay day, TimeOnly startTime, TimeOnly endTime)
        {

            if (startTime >= endTime)
                return DoctorWorkScheduleErrors.InvalidTimeRange;

            return new DoctorWorkSchedule(id, doctorId, doctor, day, startTime, endTime);
        }

        public Result<Updated> Update(WorkDay day, TimeOnly startTime, TimeOnly endTime)
        {
            if (startTime >= endTime)
                return DoctorWorkScheduleErrors.InvalidTimeRange;

            dayofWeek = day;
            StartTime = startTime;
            EndTime = endTime;

            return Result.Updated;
        }

        public Result<Success> CanSchedule(TimeOnly otherStart, TimeOnly otherEnd)
        {
            if (otherStart >= otherEnd)
                return DoctorWorkScheduleErrors.InvalidTimeRange;

            if ((StartTime < otherEnd && EndTime > otherStart))
                return DoctorWorkScheduleErrors.OverlappingSchedule;

            return Result.Success;
        }



    }

}
