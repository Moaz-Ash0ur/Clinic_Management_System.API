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

        public Result<Updated> Update(DateTime dateOfBirth, Gender gender, bool isActive)
        {
            if (dateOfBirth == default)
                return PatientErrors.DateOfBirthRequired;

            DateOfBirth = dateOfBirth;
            Gender = gender;
            IsActive = isActive;

            return Result.Updated;
        }

        public Result<Updated> UpsertMedicalRecords(List<MedicalRecord> incomingRecords)
        {
            _medicalRecords.RemoveAll(existing => incomingRecords.All(r => r.Id != existing.Id));

            foreach (var incoming in incomingRecords)
            {
                var existing = _medicalRecords.FirstOrDefault(r => r.Id == incoming.Id);
                if (existing is null)
                {
                    _medicalRecords.Add(incoming);
                }
                else
                {
                    existing = incoming; 
                }
            }

            return Result.Updated;
        }
    }




}
