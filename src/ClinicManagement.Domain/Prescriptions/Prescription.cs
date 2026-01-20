using ClinicManagement.Domain.Common;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Patients;
using ClinicManagement.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Prescriptions
{
    public sealed class Prescription : AuditableEntity
    {
        public Guid PatientId { get; private set; }
        public Patient? Patient { get; private set; }

        public Guid SessionId { get; private set; }
        public Session? Session { get; private set; }

        public string MedicationName { get; private set; }  
        public string Dosage { get; private set; }           
        public string Description { get; private set; }     

        private Prescription() { } 

        private Prescription(Guid id, Guid patientId, Guid sessionId, string medicationName, string dosage, string description)
            : base(id)
        {
            PatientId = patientId;
            SessionId = sessionId;
            MedicationName = medicationName;
            Dosage = dosage;
            Description = description;
        }

        public static Result<Prescription> Create(Guid id, Guid patientId, Guid sessionId, string medicationName, string dosage, string description)
        {
            if (string.IsNullOrWhiteSpace(medicationName))
                return PrescriptionErrors.MedicationNameRequired;

            if (string.IsNullOrWhiteSpace(dosage))
                return PrescriptionErrors.DosageRequired;

            if (string.IsNullOrWhiteSpace(description))
                return PrescriptionErrors.DescriptionRequired;

            return new Prescription(id, patientId, sessionId, medicationName, dosage, description);
        }

        public Result<Updated> Update(string medicationName, string dosage, string description)
        {
            if (string.IsNullOrWhiteSpace(medicationName))
                return PrescriptionErrors.MedicationNameRequired;

            if (string.IsNullOrWhiteSpace(dosage))
                return PrescriptionErrors.DosageRequired;

            if (string.IsNullOrWhiteSpace(description))
                return PrescriptionErrors.DescriptionRequired;

            MedicationName = medicationName;
            Dosage = dosage;
            Description = description;

            return Result.Updated;
        }
    }


}
