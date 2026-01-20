using ClinicManagement.Application.Common.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Prescriptions.Dto

{
    public sealed class PrescriptionDto : BaseDto
    {
        public Guid PatientId { get; set; }
        public Guid SessionId { get; set; }
        public string MedicationName { get; set; } 
        public string Dosage { get; set; } 
        public string Description { get; set; }
    }


}
