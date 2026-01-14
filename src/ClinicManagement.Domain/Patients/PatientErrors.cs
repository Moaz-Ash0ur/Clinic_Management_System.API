using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Domain.Patients
{

    public static class PatientErrors
    {
        public static Error DateOfBirthRequired =>
            Error.Validation("Patient_DateOfBirth_Required", "Date of Birth is required");
    }


}
