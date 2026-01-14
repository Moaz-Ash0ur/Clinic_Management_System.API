using ClinicManagement.Application.Common.BaseDto;
using ClinicManagement.Domain.Enums;

namespace ClinicManagement.Application.Featuers.Appointments.Dtos
{
    public class AppointmentDto : BaseDto
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public AppointmentStatus Status { get; set; }

    }







}
