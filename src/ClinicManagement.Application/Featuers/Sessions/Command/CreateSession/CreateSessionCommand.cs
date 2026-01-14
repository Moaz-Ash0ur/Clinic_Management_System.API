using ClinicManagement.Application.Common.BaseDto;
using ClinicManagement.Application.Featuers.Sessions.Dtos;
using ClinicManagement.Domain.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagement.Application.Featuers.Sessions.Command.CreateSession
{
    

    public sealed record CreateSessionCommand(Guid AppointmentId) : IRequest<Result<SessionDto>>;


    //-----------------------------------------------------







}
