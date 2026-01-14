using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Patients;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Patients.Command.DeletePatient
{
    public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand, Result<Success>>
    {
        private readonly IRepository<Patient> _repo;
        private readonly IRepository<Appointment> _appointmentRepo;
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _uow;

        public DeletePatientCommandHandler(IIdentityService identityService, IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Patient>(); ;
            _appointmentRepo = _uow.GetRepository<Appointment>();
            _identityService = identityService;
        }

        public async Task<Result<Success>> Handle(DeletePatientCommand command, CancellationToken ct)
        {
            var patient = await _repo.GetByIdAsync(command.Id);
            if (patient == null)
                return ApplicationErrors.PatientNotFound;

            //var hasAppointments = await _appointmentRepo.AnyAsync(a => a.PatientId == command.Id && a.Status != AppointmentStatus.Cancelled);
            //if (hasAppointments)
            //    return ApplicationErrors.PatientHasActiveAppointments;

            await _uow.BeginTransactionAsync();

            try
            {
                _repo.Delete(patient);

                var identityResult = await _identityService.DeleteUser(patient.UserId);
                if (identityResult.IsError)
                {
                    await _uow.RollbackAsync();
                    return identityResult;
                }

                await _uow.CommitAsync();
                return Result.Success;
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                return Error.Failure("", ex.Message);
            }

        }



    }




}
