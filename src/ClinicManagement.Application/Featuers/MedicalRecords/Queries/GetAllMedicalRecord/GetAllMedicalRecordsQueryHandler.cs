using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.MedicalRecords.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Patients.MedicalRecords;
using MediatR;

namespace ClinicManagement.Application.Featuers.MedicalRecords.Queries.GetAllMedicalRecord
{
    public sealed class GetAllMedicalRecordsQueryHandler : IRequestHandler<GetAllMedicalRecordsQuery,Result<IReadOnlyList<MedicalRecordDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllMedicalRecordsQueryHandler(IUnitOfWork uow,IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<IReadOnlyList<MedicalRecordDto>>> Handle(GetAllMedicalRecordsQuery query,CancellationToken ct)
        {
            var repo = _uow.GetRepository<MedicalRecord>();

            var records = await repo.GetAllAsync();

            if (!records.Any())
                return ApplicationErrors.MedicalRecordNotFound;

            return _mapper.Map<List<MedicalRecordDto>>(records);
        }
    }


}
