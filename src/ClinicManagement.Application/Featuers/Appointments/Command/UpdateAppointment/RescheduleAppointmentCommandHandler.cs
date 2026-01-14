using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Appointments.Command.CreateAppointment;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Constamts;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ClinicManagement.Application.Featuers.Appointments.Command.UpdateAppointment
{
    public class RescheduleAppointmentCommandHandler : IRequestHandler<RescheduleAppointmentCommand, Result<Updated>>
     {
        private readonly IAppointmentService _appointmentService;
        private readonly IRepository<Appointment> _repo;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<CreateAppointmentCommandHandler> _logger;

        public RescheduleAppointmentCommandHandler(IAppointmentService appointmentService, IUnitOfWork uow, ILogger<CreateAppointmentCommandHandler> logger)
        {
            _appointmentService = appointmentService;
            _uow = uow;
            _repo = _uow.GetRepository<Appointment>();
            _logger = logger;
        }

        public async Task<Result<Updated>> Handle(RescheduleAppointmentCommand command, CancellationToken cancellationToken)
        {
            var appointment = await _repo.GetByIdAsync(command.AppointmentId);
            if(appointment is  null)
            {
               return ApplicationErrors.AppointmentNotFound;
            }

            if (!_appointmentService.IsValidTime(command.NewScheduledAt, command.NewScheduledAt.AddMinutes(ClinicConstants.DefaultDuration)))
            {
                _logger.LogWarning("Invalid appointment time attempted. DoctorId: {DoctorId}, PatientId: {PatientId}, ScheduledAt: {ScheduledAt}",
                    appointment.DoctorId, appointment.PatientId, appointment.ScheduledAt);
                return ApplicationErrors.InvalidTime;
            }

            if (!await _appointmentService.DoctorIsAvailableAsync(appointment.DoctorId, command.NewScheduledAt))
            {
                _logger.LogWarning("Doctor not available. DoctorId: {DoctorId}, ScheduledAt: {ScheduledAt}", appointment.DoctorId, command.NewScheduledAt);
                return ApplicationErrors.DoctorNotAvailable;
            }

            if (await _appointmentService.DoctorHasConflictAsync(appointment.DoctorId, command.NewScheduledAt))
            {
                _logger.LogWarning("Doctor has a conflicting appointment. DoctorId: {DoctorId}, ScheduledAt: {ScheduledAt}", appointment.DoctorId, command.NewScheduledAt);
                return ApplicationErrors.DoctorConflict;
            }


            appointment.Reschedule(command.NewScheduledAt);

           _repo.Update(appointment);
           await _uow.SaveChangesAsync();

            return Result.Updated; 
        }




    }




}
