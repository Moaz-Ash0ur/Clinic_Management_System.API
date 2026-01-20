using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Contracts.Requests.NewFolder
{
    public sealed class CreatePrescriptionRequest
    {
        public Guid PatientId { get; set; }
        public Guid SessionId { get; set; }

        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public sealed class UpdatePrescriptionRequest
    {
        public string MedicationName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

}
