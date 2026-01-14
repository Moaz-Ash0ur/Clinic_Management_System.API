using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Common.Mappers;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Doctors;
using MediatR;

namespace ClinicManagement.Application.Featuers.Users.Doctors.Queries.GetDoctorsQuery
{
    public class GetDoctorsQueryHandler : IRequestHandler<GetDoctorsQuery, Result<List<DoctorDto>>>
    {
        private readonly IRepository<Doctor> _doctorRepo;
        private readonly IUnitOfWork _uow;
        private readonly IIdentityService _identityService;

        public GetDoctorsQueryHandler(IUnitOfWork uow, IIdentityService identityService)
        {
            _uow = uow;
            _doctorRepo = _uow.GetRepository<Doctor>();
            _identityService = identityService;
        }


        public async Task<Result<List<DoctorDto>>> Handle(GetDoctorsQuery query, CancellationToken ct)
        {
            var doctors = await _doctorRepo.GetAllAsync();

            var users = await _identityService.GetAllUsers();

            if (users!.IsError)
            {
                return users.TopError;
            }

            return doctors.ToDtos(users!.Value);
        }


    }



}
