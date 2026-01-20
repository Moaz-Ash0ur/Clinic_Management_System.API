using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.Prescriptions.Dto;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Prescriptions;
using ClinicManagement.Domain.Sessions;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Prescriptions.Command.CreatePrescription
{
    public sealed record CreatePrescriptionCommand(
     Guid PatientId,
     Guid SessionId,
     string MedicationName,
     string Dosage,
     string Description
 ) : IRequest<Result<PrescriptionDto>>;



    public sealed class CreatePrescriptionCommandHandler : IRequestHandler<CreatePrescriptionCommand, Result<PrescriptionDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly HybridCache _cache;

        public CreatePrescriptionCommandHandler(IUnitOfWork uow, IMapper mapper, HybridCache cache)
        {
            _uow = uow;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<PrescriptionDto>> Handle(CreatePrescriptionCommand command, CancellationToken ct)
        {
            var sessionRepo = _uow.GetRepository<Session>();
            var prescriptionRepo = _uow.GetRepository<Prescription>();

            var session = await sessionRepo.FindAsync(s => s.Id == command.SessionId);
            if (session is null)
                return ApplicationErrors.SessionNotFound;

            if (session.Status != SessionStatus.Completed)
                return ApplicationErrors.SessionNotCompleted;

         
            var exists = await prescriptionRepo.AnyAsync(p => p.SessionId == session.Id);
            if (exists)
                return ApplicationErrors.PrescriptionAlreadyExists;


            var result = Prescription.Create(
                Guid.NewGuid(),
                command.PatientId,
                session.Id,
                command.MedicationName,
                command.Dosage,
                command.Description
            );

            if (result.IsError)
                return result.Errors;

            await prescriptionRepo.AddAsync(result.Value);
            await _uow.SaveChangesAsync();

            await _cache.RemoveByTagAsync("prescriptions");

            return _mapper.Map<PrescriptionDto>(result.Value);
        }


   }



}
