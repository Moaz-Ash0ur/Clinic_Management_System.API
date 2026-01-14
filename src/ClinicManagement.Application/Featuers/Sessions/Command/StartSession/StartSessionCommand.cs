using ClinicManagement.Domain.Common.Results;
using MediatR;

namespace ClinicManagement.Application.Featuers.Sessions.Command.StartSession
{
    public sealed record StartSessionCommand(Guid SessionId) : IRequest<Result<Success>>;









}
