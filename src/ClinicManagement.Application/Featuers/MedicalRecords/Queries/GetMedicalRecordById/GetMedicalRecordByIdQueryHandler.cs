using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.MedicalRecords.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Patients.MedicalRecords;
using MediatR;

namespace ClinicManagement.Application.Featuers.MedicalRecords.Queries.GetMedicalRecordById
{
    public sealed class GetMedicalRecordByIdQueryHandler : IRequestHandler<GetMedicalRecordByIdQuery, Result<MedicalRecordDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetMedicalRecordByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<MedicalRecordDto>> Handle(GetMedicalRecordByIdQuery query, CancellationToken ct)
        {
            var repo = _uow.GetRepository<MedicalRecord>();

            var record = await repo.GetByIdAsync(query.Id);

            if (record is null)
                return ApplicationErrors.MedicalRecordNotFound;

            return _mapper.Map<MedicalRecordDto>(record);
        }



    }


}
