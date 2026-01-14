using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Domain.Patients.MedicalRecords
{

    // MedicalRecordErrors.cs
    public static class MedicalRecordErrors
    {
        public static Error DiagnosisRequired =>
            Error.Validation("MedicalRecord_Diagnosis_Required", "Diagnosis is required");
    }

}
