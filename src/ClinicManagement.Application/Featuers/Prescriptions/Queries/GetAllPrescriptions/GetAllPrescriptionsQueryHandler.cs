using AutoMapper;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Prescriptions.Dto;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Prescriptions;
using MediatR;

namespace ClinicManagement.Application.Featuers.Prescriptions.Queries.GetAllPrescriptions
{
    public sealed class GetAllPrescriptionsQueryHandler : IRequestHandler<GetAllPrescriptionsQuery, Result<List<PrescriptionDto>>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetAllPrescriptionsQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<List<PrescriptionDto>>> Handle(GetAllPrescriptionsQuery query, CancellationToken ct)
        {
            var prescriptions = await _uow
                .GetRepository<Prescription>()
                .GetAllAsync();

            return _mapper.Map<List<PrescriptionDto>>(prescriptions);
        }
    }

}
