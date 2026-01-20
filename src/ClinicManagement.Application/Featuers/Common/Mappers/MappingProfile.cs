using AutoMapper;
using ClinicManagement.Application.Featuers.Appointments.Dtos;
using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Dtos;
using ClinicManagement.Application.Featuers.MedicalRecords.Command.CreateMedicalRecord;
using ClinicManagement.Application.Featuers.MedicalRecords.Dtos;
using ClinicManagement.Application.Featuers.Prescriptions.Dto;
using ClinicManagement.Application.Featuers.Sessions.Dtos;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Doctors;
using ClinicManagement.Domain.DoctorWorkSchedules;
using ClinicManagement.Domain.Patients.MedicalRecords;
using ClinicManagement.Domain.Prescriptions;
using ClinicManagement.Domain.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Common.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<DoctorWorkSchedule, DoctorScheduleDto>();
            CreateMap<Appointment, AppointmentDto>();
            CreateMap<Session, SessionDto>();
            CreateMap<MedicalRecord, MedicalRecordDto>();
            CreateMap<Prescription, PrescriptionDto>();


        }

    }
}
