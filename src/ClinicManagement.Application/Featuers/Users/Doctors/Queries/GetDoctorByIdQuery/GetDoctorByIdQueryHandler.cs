using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Common.Mappers;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Doctors;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Doctors.Queries.GetDoctorByIdQuery
{
    public class GetDoctorByIdQueryHandler : IRequestHandler<GetDoctorByIdQuery, Result<DoctorDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Doctor> _repo;
        private readonly IIdentityService _identityService;



        public GetDoctorByIdQueryHandler(IUnitOfWork uow,IIdentityService identityService)
        {
             _identityService = identityService;
            _uow = uow;
            _repo = _uow.GetRepository<Doctor>();
        }

        public async Task<Result<DoctorDto>> Handle(GetDoctorByIdQuery query, CancellationToken ct)
        {

            var doctor =  await _repo.GetByIdAsync(query.DcotorId);
            if (doctor == null)            
                return ApplicationErrors.DoctorNotFound;


            var getUserDoctor = await _identityService.GetUserByIdAsync(doctor.UserId);

           var doctorUser =  getUserDoctor.Value;

           return doctor.ToDto(doctorUser);

        }



    }



}
