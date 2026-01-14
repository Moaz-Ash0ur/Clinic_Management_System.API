using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Sessions.Dtos;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Constamts;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Sessions;
using ClinicManagement.Domain.Sessions.Attendances;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ClinicManagement.Application.Featuers.Sessions.Command.CreateSession
{
    public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, Result<SessionDto>>
    {
        private readonly IRepository<Session> _sessionRepo;
        private readonly IRepository<Appointment> _appointmentRepo;
        private readonly IRepository<Attendance> _attendanceRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSessionCommandHandler> _logger;

        public CreateSessionCommandHandler(IUnitOfWork uow, IMapper mapper, ILogger<CreateSessionCommandHandler> logger)
        {
            _uow = uow;
            _sessionRepo = _uow.GetRepository<Session>();
            _appointmentRepo = _uow.GetRepository<Appointment>();
            _mapper = mapper;
            _attendanceRepo = _uow.GetRepository<Attendance>();
            _logger = logger;
        }

        public async Task<Result<SessionDto>> Handle(CreateSessionCommand command,CancellationToken ct)
        {     
            var appointment = await _appointmentRepo.GetByIdAsync(command.AppointmentId);

            if (appointment is null) 
            {
                _logger.LogWarning(
                    "Appointment not found. AppointmentId: {AppointmentId}",
                    command.AppointmentId);

                return ApplicationErrors.AppointmentNotFound;
            }

            var now = DateTime.Now;

            var appointmentStart = appointment.ScheduledAt;
            var appointmentEndExpected = appointment.ScheduledAt
                .AddMinutes(ClinicConstants.DefaultDuration);

            if (now < appointmentStart)
            {
                _logger.LogWarning(
                    "Attempt to start session before appointment time. AppointmentId: {AppointmentId}",
                    appointment.Id);

                return ApplicationErrors.AppointmentNotStartedYet;
            }

            if (now > appointmentEndExpected)
            {
                _logger.LogWarning(
                    "Attempt to start session for expired appointment. AppointmentId: {AppointmentId}",
                    appointment.Id);

                return ApplicationErrors.AppointmentExpired;
            }
            


            if (appointment.Status != AppointmentStatus.Scheduled)
            {
                _logger.LogWarning(
                    "Appointment is not ready for session. AppointmentId: {AppointmentId}, Status: {Status}",
                    appointment.Id,
                    appointment.Status);

                return ApplicationErrors.AppointmentNotReadyForSession;
            }

           await _uow.BeginTransactionAsync();

            try
            {
                var changeStatusResult = appointment.ChangeStatus(AppointmentStatus.Confirmed);

                if (changeStatusResult.IsError)
                {
                    _logger.LogWarning(
                        "Failed to confirm appointment. AppointmentId: {AppointmentId}",
                        appointment.Id);

                    return changeStatusResult.Errors;
                }

                var sessionResult = Session.Create(Guid.NewGuid(), appointment.Id);

                if (sessionResult.IsError)
                {
                    _logger.LogWarning(
                        "Failed to create session for AppointmentId: {AppointmentId}",
                        appointment.Id);

                    return sessionResult.Errors;
                }

                var session = sessionResult.Value;

                var attendanceResult = Attendance.Create(
                    Guid.NewGuid(),
                    session.Id,
                    appointment.PatientId,
                    AttendanceStatus.Pending,
                    DateTime.UtcNow);

                if (attendanceResult.IsError)
                {
                    _logger.LogWarning(
                        "Failed to create attendance. SessionId: {SessionId}",
                        session.Id);

                    return attendanceResult.Errors;
                }

               
                _appointmentRepo.Update(appointment);
                await _sessionRepo.AddAsync(session);
                await _attendanceRepo.AddAsync(attendanceResult.Value);

                await _uow.CommitAsync();
                await _uow.SaveChangesAsync();

                _logger.LogInformation(
                    "Session created successfully. SessionId: {SessionId}",
                    session.Id);

                return _mapper.Map<SessionDto>(session);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error while creating session for AppointmentId: {AppointmentId}",
                    command.AppointmentId);

                await _uow.RollbackAsync();
                throw;
            }
        }
    }


    //-----------------------------------------------------







}
