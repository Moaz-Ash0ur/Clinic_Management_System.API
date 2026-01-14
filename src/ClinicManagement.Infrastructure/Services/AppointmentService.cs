using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Constamts;
using ClinicManagement.Domain.DoctorWorkSchedules;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Infrastructure.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository<DoctorWorkSchedule> _scheduleRepo;
        private readonly IRepository<Appointment> _appointmentRepo;
        private readonly IRepository<Patient> _patientRepo;

        private readonly IUnitOfWork _uow;

        public AppointmentService(IUnitOfWork uow)
        {
            _uow = uow;
            _scheduleRepo = _uow.GetRepository<DoctorWorkSchedule>();
            _patientRepo = _uow.GetRepository<Patient>();
            _appointmentRepo = _uow.GetRepository<Appointment>();
        }

        private DateTime GetEndTime(DateTime start) => start.AddMinutes(ClinicConstants.DefaultDuration);


        public async Task<bool> PatientHasConflictAsync(Guid patientId, DateTime start)
        {
            var end = GetEndTime(start);

            return await _appointmentRepo.AnyAsync(a =>
                a.PatientId == patientId &&
                start < a.ScheduledAt.AddMinutes(ClinicConstants.DefaultDuration) &&
                end > a.ScheduledAt
            );
        }


        public async Task<bool> DoctorIsAvailableAsync(Guid doctorId, DateTime start)
        {
            var startTime = TimeOnly.FromDateTime(start);
            var endTime = TimeOnly.FromDateTime(GetEndTime(start));
            var workDay = (WorkDay)start.DayOfWeek;

            return await _scheduleRepo.AnyAsync(s =>
                s.DoctorId == doctorId &&
                s.dayofWeek == workDay &&
                startTime >= s.StartTime &&
                endTime <= s.EndTime
            );
        }


        public async Task<bool> DoctorHasConflictAsync(Guid doctorId, DateTime start)
        {
            var end = GetEndTime(start);

            return await _appointmentRepo.AnyAsync(a =>
                a.DoctorId == doctorId &&
                start < a.ScheduledAt.AddMinutes(ClinicConstants.DefaultDuration) &&
                end > a.ScheduledAt
            );
        }

        //{
        //  "doctorId": "d3ed3dd7-b577-4d84-8f30-1f76f68c129b",
        //  "patientId": "1ce234a7-9d09-4bd4-994a-ab45209bd3eb",
        //  "scheduledAt": "2026-01-13T14:48:00.152Z"
        //}

        public bool IsValidTime(DateTime start, DateTime end)
        {
            return start > DateTime.Now && end > start;
        }

      

    }



}
