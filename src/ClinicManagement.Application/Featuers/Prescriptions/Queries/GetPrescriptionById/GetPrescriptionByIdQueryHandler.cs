using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Prescriptions.Dto;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Prescriptions;
using MediatR;

namespace ClinicManagement.Application.Featuers.Prescriptions.Queries.GetPrescriptionById
{
    public sealed class GetPrescriptionByIdQueryHandler : IRequestHandler<GetPrescriptionByIdQuery, Result<PrescriptionDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public GetPrescriptionByIdQueryHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<Result<PrescriptionDto>> Handle(GetPrescriptionByIdQuery query, CancellationToken ct)
        {
            var prescription = await _uow
                .GetRepository<Prescription>()
                .GetByIdAsync(query.Id);

            if (prescription is null)
                return ApplicationErrors.PrescriptionNotFound;


            return _mapper.Map<PrescriptionDto>(prescription);
        }


    }







}
