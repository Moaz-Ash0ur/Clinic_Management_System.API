using ClinicManagement.Application.Featuers.Sessions.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Sessions.Queries.GetSessionById
{
    public sealed record GetSessionByIdQuery(Guid SessionId)
     : IRequest<Result<SessionDto>>;

}
