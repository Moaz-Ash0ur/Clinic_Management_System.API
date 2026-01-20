using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Prescriptions;
using MediatR;

namespace ClinicManagement.Application.Featuers.Prescriptions.Command.UpdatePrescription
{
    public sealed class UpdatePrescriptionCommandHandler : IRequestHandler<UpdatePrescriptionCommand, Result<Updated>>
    {
        private readonly IUnitOfWork _uow;

        public UpdatePrescriptionCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Result<Updated>> Handle(UpdatePrescriptionCommand command, CancellationToken ct)
        {
            var prescription = await _uow
                .GetRepository<Prescription>()
                .GetByIdAsync(command.Id);

            if (prescription is null)
                return ApplicationErrors.PrescriptionNotFound;


            var updateResult = prescription.Update(
                command.MedicationName,
                command.Dosage,
                command.Description
            );

            if (updateResult.IsError)
                return updateResult.Errors;

            await _uow.SaveChangesAsync();

            return Result.Updated;
        }


    }

}
