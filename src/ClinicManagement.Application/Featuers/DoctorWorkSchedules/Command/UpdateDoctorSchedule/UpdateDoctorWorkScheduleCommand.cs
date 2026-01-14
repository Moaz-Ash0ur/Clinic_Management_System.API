using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using MediatR;


namespace ClinicManagement.Application.Featuers.DoctorWorkSchedules.Command.UpdateDoctorSchedule
{
    public sealed record UpdateDoctorWorkScheduleCommand(
        Guid Id,
        WorkDay Day, 
        TimeOnly StartTime,
        TimeOnly EndTime) : IRequest<Result<Updated>>;



}
