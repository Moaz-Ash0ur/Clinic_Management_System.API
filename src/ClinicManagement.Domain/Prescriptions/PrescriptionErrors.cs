using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Domain.Prescriptions
{

    public static class PrescriptionErrors
    {
        public static Error MedicationNameRequired =>
       Error.Validation("Prescription_MedicationName_Required", "Medication name is required.");

        public static Error DosageRequired =>
            Error.Validation("Prescription_Dosage_Required", "Dosage is required.");

        public static Error DescriptionRequired =>
            Error.Validation("Prescription_Description_Required", "Description is required.");
    }

}
