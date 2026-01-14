using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Appointments.Dtos;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Constamts;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Doctors;
using ClinicManagement.Domain.Patients;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Featuers.Appointments.Command.CreateAppointment
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, Result<AppointmentDto>>
    {

        private readonly ILogger<CreateAppointmentCommandHandler> _logger;
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Appointment> _appointmentRepo;
        private readonly IRepository<Doctor> _doctorRepo;
        private readonly IRepository<Patient> _patientRepo;
        private readonly IMapper _mapper;
        private readonly IAppointmentService _appointmentService;


        public CreateAppointmentCommandHandler(IUnitOfWork uow, ILogger<CreateAppointmentCommandHandler> logger,
            IMapper mapper, IAppointmentService appointmentService)
        {
            _uow = uow;
            _appointmentRepo = _uow.GetRepository<Appointment>();
            _doctorRepo = _uow.GetRepository<Doctor>();
            _patientRepo = _uow.GetRepository<Patient>();
            _mapper = mapper;
            _logger = logger;
            _appointmentService = appointmentService;
        }

        public async Task<Result<AppointmentDto>> Handle(CreateAppointmentCommand command, CancellationToken ct)
        {
            var doctor = await _doctorRepo.AnyAsync(d => d.Id ==  command.DoctorId);
            if (!doctor)
            {
                _logger.LogWarning("Attempt to create appointment failed. Doctor not found. DoctorId: {DoctorId}", command.DoctorId);
                return ApplicationErrors.DoctorNotFound;
            }

            var patient = await _patientRepo.AnyAsync(p => p.Id == command.PatientId);
            if (!patient)
            {
                _logger.LogWarning("Attempt to create appointment failed. Patient not found. PatientId: {PatientId}", command.PatientId);
                return ApplicationErrors.PatientNotFound;
            }

            var duration = ClinicConstants.DefaultDuration;

            if (!_appointmentService.IsValidTime(command.ScheduledAt, command.ScheduledAt.AddMinutes(duration)))
            {
                _logger.LogWarning("Invalid appointment time attempted. DoctorId: {DoctorId}, PatientId: {PatientId}, ScheduledAt: {ScheduledAt}",
                    command.DoctorId, command.PatientId, command.ScheduledAt);
                return ApplicationErrors.InvalidTime;
            }

            if (!await _appointmentService.DoctorIsAvailableAsync(command.DoctorId, command.ScheduledAt))
            {
                _logger.LogWarning("Doctor not available. DoctorId: {DoctorId}, ScheduledAt: {ScheduledAt}", command.DoctorId, command.ScheduledAt);
                return ApplicationErrors.DoctorNotAvailable;
            }

            if (await _appointmentService.DoctorHasConflictAsync(command.DoctorId, command.ScheduledAt))
            {
                _logger.LogWarning("Doctor has a conflicting appointment. DoctorId: {DoctorId}, ScheduledAt: {ScheduledAt}", command.DoctorId, command.ScheduledAt);
                return ApplicationErrors.DoctorConflict;
            }

            if (await _appointmentService.PatientHasConflictAsync(command.PatientId, command.ScheduledAt))
            {
                _logger.LogWarning("Patient has a conflicting appointment. PatientId: {PatientId}, ScheduledAt: {ScheduledAt}", command.PatientId, command.ScheduledAt);
                return ApplicationErrors.PatientConflict;
            }

            var createResult = Appointment.Create(
                Guid.NewGuid(),
                command.DoctorId,
                command.PatientId,
                command.ScheduledAt,
                TimeSpan.FromMinutes(duration));

            if (createResult.IsError)
            {
                _logger.LogError("Appointment creation failed due to domain rules. Errors: {Errors}", createResult.Errors);
                return createResult.Errors;
            }

            var appointment = createResult.Value;

            try
            {
                await _appointmentRepo.AddAsync(appointment);
                await _uow.SaveChangesAsync();
                _logger.LogInformation("Appointment created successfully. Id: {AppointmentId}, DoctorId: {DoctorId}, PatientId: {PatientId}, ScheduledAt: {ScheduledAt}",
                    appointment.Id, appointment.DoctorId, appointment.PatientId, appointment.ScheduledAt);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while saving appointment to database. DoctorId: {DoctorId}, PatientId: {PatientId}, ScheduledAt: {ScheduledAt}",
                    command.DoctorId, command.PatientId, command.ScheduledAt);
                throw;
            }

               return _mapper.Map<AppointmentDto>(appointment);
        }




    }



}
