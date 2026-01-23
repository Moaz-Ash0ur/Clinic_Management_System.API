using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Appointments.Dtos;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Constamts;
using ClinicManagement.Domain.DoctorWorkSchedules;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Patients;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUnitOfWork _uow;

        public AppointmentService(IUnitOfWork uow)
        {
            _uow = uow;
            _scheduleRepo = _uow.GetRepository<DoctorWorkSchedule>();
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

      
        public bool IsValidTime(DateTime start, DateTime end)
        {
            return start > DateTime.Now && end > start;
        }


        public async Task<List<Appointment>> GetScheduledAppointment()
        {         
            var now = DateTime.Now;

            //need reCheck for Select Col what need just
            //reminderSent ScheduledAt PatientName

            return await _appointmentRepo
             .GetQueryable()
             .Include(a => a.Patient)
                .ThenInclude(p => p!.User)
             .Where(a =>
                 a.Status == AppointmentStatus.Scheduled &&
                 !a.reminderSent &&
                 a.ScheduledAt <= now.AddHours(24))
                        
             .ToListAsync();

        }
    
    }




}
