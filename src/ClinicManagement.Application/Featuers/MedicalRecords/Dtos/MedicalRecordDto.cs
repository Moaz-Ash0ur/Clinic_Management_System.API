using ClinicManagement.Application.Common.BaseDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.MedicalRecords.Dtos
{
    public class MedicalRecordDto : BaseDto
    {
        public Guid SessionId { get; set; }
        public Guid PatientId { get; set; }
        public string Diagnosis { get; set; }
        public string? Notes { get; private set; }
    }
}
