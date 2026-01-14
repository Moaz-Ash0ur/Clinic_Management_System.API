using ClinicManagement.Domain.Common.Results;

namespace ClinicManagement.Domain.Doctors
{
    // DoctorErrors.cs
    public static class DoctorErrors
    {
        public static Error SpecializationRequired =>
            Error.Validation("Doctor_Specialization_Required", "Specialization is required");
    }






}
