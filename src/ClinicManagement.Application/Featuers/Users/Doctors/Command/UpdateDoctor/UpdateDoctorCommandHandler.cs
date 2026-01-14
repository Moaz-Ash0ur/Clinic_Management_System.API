using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Doctors;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Doctors.Command.UpdateDoctor
{
    public class UpdateDoctorCommandHandler : IRequestHandler<UpdateDoctorCommand, Result<Success>>
    {
        private readonly IRepository<Doctor> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IIdentityService _identityService;


        public UpdateDoctorCommandHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Doctor>();
            _identityService = identityService;
        }

        public async Task<Result<Success>> Handle(UpdateDoctorCommand command, CancellationToken ct)
        {
            var patient = await _repo.GetByIdAsync(command.Id);
            if (patient == null)
                return ApplicationErrors.DoctorNotFound;


            var patientUpdatedResult = await _identityService
                .UpdateUser(patient.UserId, command.FirstName,
                  command.LastName, command.Email);

            if (patientUpdatedResult.IsError)
                return Error.Failure("Faild to Update Doctor Data");

            return Result.Success;
        }



    }





}
