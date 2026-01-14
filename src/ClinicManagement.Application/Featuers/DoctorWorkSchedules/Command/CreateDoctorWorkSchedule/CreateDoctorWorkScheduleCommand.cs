using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Enums;
using MediatR;



namespace ClinicManagement.Application.Featuers.DoctorWorkSchedules.Command.CreateDoctorWorkSchedule
{
    public sealed record  CreateDoctorWorkScheduleCommand(
      Guid DoctorId,
      WorkDay Day,
      TimeOnly StartTime,
      TimeOnly EndTime) : IRequest<Result<DoctorScheduleDto>>;




}
