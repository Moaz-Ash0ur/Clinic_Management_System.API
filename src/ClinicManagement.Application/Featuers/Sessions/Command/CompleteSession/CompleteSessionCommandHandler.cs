using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Sessions;
using ClinicManagement.Domain.Sessions.Attendances;
using MediatR;

namespace ClinicManagement.Application.Featuers.Sessions.Command.CompleteSession
{
    public class CompleteSessionCommandHandler : IRequestHandler<CompleteSessionCommand, Result<Success>>
    {
        private readonly IRepository<Session> _sessionRepo;
        private readonly IRepository<Appointment> _appointmentRepo;
        private readonly IUnitOfWork _uow;

        public CompleteSessionCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
            _sessionRepo = uow.GetRepository<Session>();
            _appointmentRepo = _uow.GetRepository<Appointment>();
        }

        public async Task<Result<Success>> Handle(CompleteSessionCommand command,CancellationToken ct)
        {
            var session = await _sessionRepo.GetByIdAsync(command.SessionId);
            if (session is null)
                return ApplicationErrors.SessionNotFound;

            await _uow.BeginTransactionAsync();

            try
            {
                session.AddDoctorNotes(command.DoctorNotes ?? string.Empty);

                var result = session.Complete(DateTime.UtcNow);
                if (result.IsError)
                {
                    return result.Errors;
                }

                var appointment = await _appointmentRepo.GetByIdAsync(session.AppointmentId);
                if (appointment is null)
                {
                    return ApplicationErrors.AppointmentNotFound;
                }

                var appointmentResult = appointment.ChangeStatus(AppointmentStatus.Completed);
                if (appointmentResult.IsError)
                {
                    return appointmentResult.Errors;
                }

                _sessionRepo.Update(session);
                _appointmentRepo.Update(appointment);

                await _uow.CommitAsync();
                await _uow.SaveChangesAsync();

                return Result.Success;
            }
            catch
            {
                await _uow.RollbackAsync();
                throw;
            }
        }
    }


    //-----------------------------------------------------







}
