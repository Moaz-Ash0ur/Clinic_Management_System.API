using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Domain.Prescriptions
{

    public static class PrescriptionErrors
    {
        public static Error CreatedAtRequired =>
            Error.Validation("Prescription_CreatedAt_Required", "Creation date is required");
    }

}
