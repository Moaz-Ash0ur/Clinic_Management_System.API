using ClinicManagement.Domain.Common.Results;
using MediatR;

namespace ClinicManagement.Application.Featuers.Sessions.Command.CompleteSession
{
    public sealed record CompleteSessionCommand(Guid SessionId,string? DoctorNotes) : IRequest<Result<Success>>;









}
