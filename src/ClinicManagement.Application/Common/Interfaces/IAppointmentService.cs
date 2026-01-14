using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Queries.NewFolder1;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Constamts;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.DoctorWorkSchedules;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Patients;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace ClinicManagement.Application.Common.Interfaces
{
    public interface IAppointmentService
    {
         Task<bool> PatientHasConflictAsync(Guid patientId, DateTime start);
         Task<bool> DoctorIsAvailableAsync(Guid doctorId, DateTime start);
         Task<bool> DoctorHasConflictAsync(Guid doctorId, DateTime start);
        bool IsValidTime(DateTime start, DateTime end);
    }






}
