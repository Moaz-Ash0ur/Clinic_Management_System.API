using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.DoctorWorkSchedules;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace ClinicManagement.Application.Featuers.DoctorWorkSchedules.Command.UpdateDoctorSchedule
{
    public sealed class UpdateDoctorWorkScheduleHandler : IRequestHandler<UpdateDoctorWorkScheduleCommand, Result<Updated>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<DoctorWorkSchedule> _repo;

        public UpdateDoctorWorkScheduleHandler(IUnitOfWork uow)
        {
            _uow = uow;
            _repo = _uow.GetRepository<DoctorWorkSchedule>();
        }

        public async Task<Result<Updated>> Handle(UpdateDoctorWorkScheduleCommand command, CancellationToken ct)
        {
            var schedule = await _repo.GetByIdAsync(command.Id);
            if (schedule is null) return ApplicationErrors.ScheduleNotFound;

            var otherSchedules = await _repo.GetAsync(s => s.DoctorId == schedule.DoctorId 
            && s.dayofWeek == command.Day && s.Id != command.Id);


            foreach (var s in otherSchedules)
            {
                var check = s.CanSchedule(command.StartTime, command.EndTime);
                if (check.IsError) return check.Errors;
            }

            var result = schedule.Update(command.Day, command.StartTime, command.EndTime);
            if (result.IsError) return result.Errors;


    //        var hasAppointments = await _context.Appointments.AnyAsync(a =>
    //a.DoctorId == schedule.DoctorId &&
    //a.AppointmentDate.WorkDay == day &&
    //a.StartTime < endTime &&
    //a.EndTime > startTime,
    //ct);

    //        if (hasAppointments)
    //            throw new ConflictException("Schedule has existing appointments");

            await _uow.SaveChangesAsync();
            return Result.Updated;
        }
    }





}
