using ClinicManagement.Application.Featuers.Prescriptions.Dto;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Common.Results.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ClinicManagement.Application.Featuers.Prescriptions.Queries.GetPrescriptionById
{
    public sealed record GetPrescriptionByIdQuery(Guid Id) : IRequest<Result<PrescriptionDto>>;







}
