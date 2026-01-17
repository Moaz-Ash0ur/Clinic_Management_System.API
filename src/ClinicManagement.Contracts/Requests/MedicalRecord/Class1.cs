using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Contracts.Requests.MedicalRecord
{
    public class CreateMedicalRecordRequest
    {
        public Guid SessionId { get; set; }
        public Guid PatientId { get; set; }
        public string Diagnosis { get; set; }
        public string? Notes { get; private set; }
    }

    public class UpdateMedicalRecordRequest
    {
        public string Diagnosis { get; set; }
        public string? Notes { get; private set; }
    }
}
