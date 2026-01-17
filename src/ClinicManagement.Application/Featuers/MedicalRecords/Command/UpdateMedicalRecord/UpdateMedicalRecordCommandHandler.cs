using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Patients.MedicalRecords;
using MediatR;
using Microsoft.Extensions.Caching.Hybrid;

namespace ClinicManagement.Application.Featuers.MedicalRecords.Command.UpdateMedicalRecord
{
    public sealed class UpdateMedicalRecordCommandHandler : IRequestHandler<UpdateMedicalRecordCommand, Result<Updated>>
    {
        private readonly IUnitOfWork _uow;
        private readonly HybridCache _cache;

        public UpdateMedicalRecordCommandHandler(IUnitOfWork uow, HybridCache cache)
        {
            _uow = uow;
            _cache = cache;
        }

        public async Task<Result<Updated>> Handle(UpdateMedicalRecordCommand command,CancellationToken ct)
        {
            var repo = _uow.GetRepository<MedicalRecord>();

            var record = await repo.GetByIdAsync(command.MedicalRecordId);

            if (record is null)
                return ApplicationErrors.MedicalRecordNotFound;

            var updateResult = record.Update(
                command.Diagnosis,
                command.Notes);

            if (updateResult.IsError)
                return updateResult.Errors;

             repo.Update(record);
            await _uow.SaveChangesAsync();

            await _cache.RemoveByTagAsync("medical-records");

            return Result.Updated;
        }
    }


}
