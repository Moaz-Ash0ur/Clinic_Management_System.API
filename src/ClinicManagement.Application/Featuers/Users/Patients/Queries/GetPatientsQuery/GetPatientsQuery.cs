using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Featuers.Users.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Users.Patients.Queries.GetPatientsQuery
{

    public record GetPatientsQuery() : IRequest<Result<List<PatientDto>>>;




}
