using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Common.Mappers;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Patients;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Patients.Queries.GetPatientByIdQuery
{
    public class GetPatientByIdQueryHandler : IRequestHandler<GetPatientByIdQuery, Result<PatientDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Patient> _repo;
        private readonly IIdentityService _identityService;



        public GetPatientByIdQueryHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            _identityService = identityService;
            _uow = uow;
            _repo = _uow.GetRepository<Patient>();
        }

        public async Task<Result<PatientDto>> Handle(GetPatientByIdQuery query, CancellationToken ct)
        {

            var patient = await _repo.GetByIdAsync(query.PatientId);
            if (patient == null)
                return ApplicationErrors.DoctorNotFound;

            var getUserPatient = await _identityService.GetUserByIdAsync(patient.UserId);

           return patient.ToDto(getUserPatient.Value);

        }



    }

}
