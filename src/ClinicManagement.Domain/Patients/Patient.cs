using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Identity;
using ClinicManagement.Domain.Patients.MedicalRecords;
using ClinicManagement.Domain.Sessions.Attendances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Domain.Patients
{ 
    public sealed class Patient : AuditableEntity 
    {
        public string UserId { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public Gender Gender { get; private set; }
        public bool IsActive { get; private set; } = true;
        public AppUser User { get; set; }

        private readonly List<Appointment> _appointments = new();
        public IEnumerable<Appointment> Appointments => _appointments.AsReadOnly();

        private readonly List<MedicalRecord> _medicalRecords = new();
        public IEnumerable<MedicalRecord> MedicalRecords => _medicalRecords.AsReadOnly();
        public ICollection<Attendance> Attendances { get; set; }

        private Patient() { }

        private Patient(Guid id, string userId, DateTime dateOfBirth, Gender gender)
            : base(id)
        {
            UserId = userId;
            DateOfBirth = dateOfBirth;
            Gender = gender;
        }


        public static Result<Patient> Create(Guid id, string userId, DateTime dateOfBirth, Gender gender)
        {
            if (dateOfBirth == default)
                return PatientErrors.DateOfBirthRequired;

            return new Patient(id, userId, dateOfBirth, gender);
        }
        
    }




}
