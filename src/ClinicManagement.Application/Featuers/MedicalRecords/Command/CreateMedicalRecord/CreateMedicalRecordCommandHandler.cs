using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.MedicalRecords.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using ClinicManagement.Domain.Patients.MedicalRecords;
using ClinicManagement.Domain.Sessions;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace ClinicManagement.Application.Featuers.MedicalRecords.Command.CreateMedicalRecord
{
    public sealed class CreateMedicalRecordCommandHandler : IRequestHandler<CreateMedicalRecordCommand, Result<MedicalRecordDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly HybridCache _cache;

        public CreateMedicalRecordCommandHandler(IUnitOfWork uow, IMapper mapper, HybridCache cache)
        {
            _uow = uow;
            _mapper = mapper;
            _cache = cache;
        }
        //ba9137d7-48b3-4ad3-9912-397c3b071eaf
        public async Task<Result<MedicalRecordDto>> Handle(CreateMedicalRecordCommand command, CancellationToken ct)
        {
            var sessionRepo = _uow.GetRepository<Session>();
            var recordRepo = _uow.GetRepository<MedicalRecord>();

            var session = await sessionRepo.FindAsync(s => s.Id == command.SessionId);

            if (session is null)
                return ApplicationErrors.SessionNotFound;

            if (session.Status != SessionStatus.Completed)
                return ApplicationErrors.SessionNotCompleted;

            var exists = await recordRepo.AnyAsync(r => r.SessionId == session.Id);

            if (exists)
                return ApplicationErrors.MedicalRecordAlreadyExists;

            var result = MedicalRecord.Create(
                Guid.NewGuid(),
                command.PatientId,
                session.Id,
                command.Diagnosis,
                command.Notes);

            if (result.IsError)
                return result.Errors;

            await recordRepo.AddAsync(result.Value);
            await _uow.SaveChangesAsync();

            await _cache.RemoveByTagAsync("medical-records");

            return _mapper.Map<MedicalRecordDto>(result.Value);           
        }
    }




}
