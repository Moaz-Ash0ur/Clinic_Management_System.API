using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Doctors;
using ClinicManagement.Domain.Enums;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Doctors.Command.DeleteDoctor
{
    public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand, Result<Success>>
    {
        private readonly IRepository<Doctor> _repo;
        private readonly IRepository<Appointment> _appointmentRepo;
        private readonly IIdentityService _identityService;
        private readonly IUnitOfWork _uow;

        public DeleteDoctorCommandHandler(IIdentityService identityService, IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Doctor>(); ;
            _appointmentRepo = _uow.GetRepository<Appointment>();
            _identityService = identityService;
        }

        public async Task<Result<Success>> Handle(DeleteDoctorCommand command, CancellationToken ct)
        {

             var forbiddenStatuses = new List<AppointmentStatus>
             {
                 AppointmentStatus.Scheduled,
                 AppointmentStatus.Confirmed
             };


            var doctor = await _repo.GetByIdAsync(command.Id);
            if (doctor == null)
                return ApplicationErrors.DoctorNotFound;

            //var hasAppointments = await _appointmentRepo.AnyAsync(a => a.DoctorId == command.Id && forbiddenStatuses.Contains(a.Status));
            //if (hasAppointments)
            //    return ApplicationErrors.DoctorHasActiveAppointments;

            await _uow.BeginTransactionAsync();

            try
            {
                 _repo.Delete(doctor);

                var identityResult = await _identityService.DeleteUser(doctor.UserId);
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
                return Error.Failure("",ex.Message);
            }

        }
    }




}
