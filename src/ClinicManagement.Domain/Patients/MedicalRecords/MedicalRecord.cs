using ClinicManagement.Domain.Common;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Patients;
using ClinicManagement.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Patients.MedicalRecords
{

    public sealed class MedicalRecord : AuditableEntity
    {
        public Guid PatientId { get; private set; }
        public Patient? Patient { get; private set; } 

        public Guid SessionId { get; private set; }
        public Session? Session { get; private set; } 

        public string Diagnosis { get; private set; }
        public string? Notes { get; private set; }

        private MedicalRecord() { }

        private MedicalRecord(Guid id, Guid patientId, Patient patient, Guid sessionId, Session session,
            string diagnosis, string? notes)
            : base(id)
        {
            PatientId = patientId;
            Patient = patient;
            SessionId = sessionId;
            Session = session;
            Diagnosis = diagnosis;
            Notes = notes;
        }

        public static Result<MedicalRecord> Create(Guid id, Guid patientId, Patient patient,
            Guid sessionId, Session session, string diagnosis, string? notes)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
                return MedicalRecordErrors.DiagnosisRequired;

            return new MedicalRecord(id, patientId, patient, sessionId, session, diagnosis, notes);
        }

        public Result<Updated> Update(string diagnosis, string? notes)
        {
            if (string.IsNullOrWhiteSpace(diagnosis))
                return MedicalRecordErrors.DiagnosisRequired;

            Diagnosis = diagnosis;
            Notes = notes;

            return Result.Updated;
        }
    }

}
