using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.MedicalRecords.Command.UpdateMedicalRecord
{
    public sealed record UpdateMedicalRecordCommand(Guid MedicalRecordId,string Diagnosis, string? Notes) : IRequest<Result<Updated>>;


}
