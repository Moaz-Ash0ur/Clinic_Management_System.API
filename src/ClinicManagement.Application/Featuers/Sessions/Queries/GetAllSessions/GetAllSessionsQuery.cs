using ClinicManagement.Application.Featuers.Sessions.Command.CreateSession;
using ClinicManagement.Application.Featuers.Sessions.Dtos;
using ClinicManagement.Domain.Appointments;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Sessions.Attendances;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Sessions.Queries.GetAllSessions
{
    public sealed record GetAllSessionsQuery()
     : IRequest<Result<List<SessionDto>>>;

}
