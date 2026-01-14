using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Sessions;
using MediatR;

namespace ClinicManagement.Application.Featuers.Appointments.Command.DeleteAppointment
{
    public class CancelAppointmentHandler : IRequestHandler<CancelAppointmentCommand, Result<Success>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Appointment> _repo;
        private readonly IRepository<Session> _sessionRepo;

        public CancelAppointmentHandler(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Appointment>();
            _sessionRepo = _uow.GetRepository<Session>();
        }

        public async Task<Result<Success>> Handle(CancelAppointmentCommand command, CancellationToken ct)
        {
            var appointment = await _repo.GetByIdAsync(command.AppointmentId);
            if (appointment == null)
                return ApplicationErrors.AppointmentNotFound;

            var existingSession = await _sessionRepo.AnyAsync(s => s.AppointmentId == command.AppointmentId);
            if (existingSession)
                return ApplicationErrors.InvalidAppointmentCancel;


            var resultCanceld = appointment.ChangeStatus(AppointmentStatus.Cancelled);

            _repo.Update(appointment);
            await _uow.SaveChangesAsync();

            return Result.Success;
        }
    }







}
