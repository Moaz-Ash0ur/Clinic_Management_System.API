using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Patients;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Patients.Command.UpdatePatient
{
    public class UpdatePatientCommandHandler : IRequestHandler<UpdatePatientCommand, Result<Success>>
    {
        private readonly IRepository<Patient> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IIdentityService _identityService;


        public UpdatePatientCommandHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            _uow = uow;
            _repo = _uow.GetRepository<Patient>();
            _identityService = identityService;
        }

        public async Task<Result<Success>> Handle(UpdatePatientCommand command, CancellationToken ct)
        {
            var patient = await _repo.GetByIdAsync(command.Id);
            if (patient == null)
                return ApplicationErrors.PatientNotFound;


            var patientUpdatedResult = await _identityService
                .UpdateUser(patient.UserId,command.FirstName, 
                  command.LastName, command.Email);

            if (patientUpdatedResult.IsError)
                return Error.Failure("Faild to Update Patient Data");
            
            return Result.Success;
        }



    }

}
