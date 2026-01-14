using ClinicManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Contracts.Requests.DoctorSchedules
{
    public class CreateDoctorScheduleRequest
    {
        public Guid DoctorId { get; set; }
        public WorkDay Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }

    public class UpdateDoctorScheduleRequest
    {
        public Guid DoctorId { get; set; }
        public WorkDay Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
