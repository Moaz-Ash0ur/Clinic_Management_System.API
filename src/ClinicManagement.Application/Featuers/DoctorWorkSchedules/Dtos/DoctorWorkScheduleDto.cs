using ClinicManagement.Application.Common.BaseDto;
using ClinicManagement.Domain.Doctors;
using ClinicManagement.Domain.Enums;

namespace ClinicManagement.Application.Featuers.DoctorWorkSchedules.Dtos
{
    public class DoctorScheduleDto : BaseDto
    {
        public Guid DoctorId { get; set; }
        public WorkDay dayofWeek { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

    }
        
    }
