using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Common.Mappers;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Patients;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Patients.Queries.GetPatientsQuery
{
    public class GetPatientQueryHandler : IRequestHandler<GetPatientsQuery, Result<List<PatientDto>>>
    {
        private readonly IRepository<Patient> _patientRepo;
        private readonly IUnitOfWork _uow;
        private readonly IIdentityService _identityService;

        public GetPatientQueryHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            _uow = uow;
            _patientRepo = _uow.GetRepository<Patient>();
            _identityService = identityService;
        }


        public async Task<Result<List<PatientDto>>> Handle(GetPatientsQuery query, CancellationToken ct)
        {
            var patients = await _patientRepo.GetAllAsync();

            var users = await _identityService.GetAllUsers();

            if (users!.IsError)
            {
                return users.TopError;
            }
           
          return patients.ToDtos(users!.Value);
        }


    }




}
