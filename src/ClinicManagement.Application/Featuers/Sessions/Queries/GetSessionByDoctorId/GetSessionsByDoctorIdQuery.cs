using ClinicManagement.Application.Featuers.Sessions.Command.StartSession;
using ClinicManagement.Application.Featuers.Sessions.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Sessions.Attendances;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Sessions.Queries.GetSessionByDoctorId
{
    public sealed record GetSessionsByDoctorIdQuery(Guid DoctorId) : IRequest<Result<List<SessionDto>>>;


}
