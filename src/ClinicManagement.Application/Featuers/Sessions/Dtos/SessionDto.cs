using ClinicManagement.Application.Common.BaseDto;
using ClinicManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Sessions.Dtos
{
    public class SessionDto : BaseDto
    {
        public Guid AppointmentId { get;  set; }
        public SessionStatus Status { get;  set; }
        public DateTime? ActualStartTime { get;  set; }
        public DateTime? ActualEndTime { get;  set; }
        public string? DoctorNotes { get;  set; }

    }
}
