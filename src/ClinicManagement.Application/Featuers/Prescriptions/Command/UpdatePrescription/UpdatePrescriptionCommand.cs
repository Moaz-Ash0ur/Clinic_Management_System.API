using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Prescriptions.Command.UpdatePrescription
{

    public sealed record UpdatePrescriptionCommand(
    Guid Id,
    string MedicationName,
    string Dosage,
    string Description
) : IRequest<Result<Updated>>;

}
