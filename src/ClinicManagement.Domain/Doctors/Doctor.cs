using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.DoctorWorkSchedules;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Roles;

namespace ClinicManagement.Domain.Doctors
{
    public class Doctor : AuditableEntity
    {
        public string UserId { get; private set; }
        public string Specialization { get; private set; }
        public bool IsActive { get; private set; } = true;

        private readonly List<Appointment> _appointments = new();
        public IEnumerable<Appointment> Appointments => _appointments.AsReadOnly();

        private readonly List<DoctorWorkSchedule> _workSchedules = new();
        public IEnumerable<DoctorWorkSchedule> WorkSchedules => _workSchedules.AsReadOnly();

        private Doctor() { }

        private Doctor(Guid id, string userId, string specialization) : base(id)
        {       
            UserId = userId;
            Specialization = specialization;
        }

        public static Result<Doctor> Create(Guid id, string userId, string specialization)
        {
            if (string.IsNullOrWhiteSpace(specialization))
                return DoctorErrors.SpecializationRequired;

            return new Doctor(id, userId, specialization);
        }

        public Result<Updated> Update(string specialization, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(specialization))
                return DoctorErrors.SpecializationRequired;

            Specialization = specialization;
            IsActive = isActive;

            return Result.Updated;
        }

        public Result<Updated> UpsertWorkSchedules(List<DoctorWorkSchedule> incomingSchedules)
        {
            _workSchedules.RemoveAll(existing => incomingSchedules.All(s => s.Id != existing.Id));

            foreach (var incoming in incomingSchedules)
            {
                var existing = _workSchedules.FirstOrDefault(s => s.Id == incoming.Id);
                if (existing is null)
                {
                    _workSchedules.Add(incoming);
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
