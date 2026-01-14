using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Contracts.Requests.Appointments
{

    public class CreateAppointmentRequest
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime ScheduledAt { get; set; }
    }

    public class UpdateAppointmentRequest
    {
        public DateTime ScheduledAt { get; set; }
    }


}
