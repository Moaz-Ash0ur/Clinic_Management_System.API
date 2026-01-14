using AutoMapper;
using ClinicManagement.Application.Common.Errors;
using ClinicManagement.Application.Common.Interfaces;
using ClinicManagement.Application.Featuers.DoctorWorkSchedules.Dtos;
using ClinicManagement.Domain.Common.Results;
using ClinicManagement.Domain.Doctors;
using ClinicManagement.Domain.DoctorWorkSchedules;
using MediatR;

namespace ClinicManagement.Application.Featuers.DoctorWorkSchedules.Command.CreateDoctorWorkSchedule
{
    public sealed class CreateDoctorWorkScheduleHandler : IRequestHandler<CreateDoctorWorkScheduleCommand, Result<DoctorScheduleDto>>
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<Doctor> _doctorRepo;
        private readonly IRepository<DoctorWorkSchedule> _repo;
        private readonly IMapper _mapper;


        public CreateDoctorWorkScheduleHandler(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _repo = _uow.GetRepository<DoctorWorkSchedule>();
            _doctorRepo = _uow.GetRepository<Doctor>();
            _mapper = mapper;
        }

        public async Task<Result<DoctorScheduleDto>> Handle(CreateDoctorWorkScheduleCommand command, CancellationToken ct)
        {
            var doctor = await _doctorRepo.GetByIdAsync(command.DoctorId);
            if (doctor is null)                
                return ApplicationErrors.DoctorNotFound;
            

            var existingSchedules = await _repo.GetAsync(s => s.DoctorId == command.DoctorId && s.dayofWeek == command.Day);
            foreach (var schedule in existingSchedules)
            {
                var check = schedule.CanSchedule(command.StartTime, command.EndTime);
                if (check.IsError) return check.Errors;
            }

            var scheduleResult = DoctorWorkSchedule.Create(Guid.NewGuid(), doctor.Id, doctor, command.Day, command.StartTime, command.EndTime);
            if (scheduleResult.IsError) return scheduleResult.Errors;

            await _repo.AddAsync(scheduleResult.Value);
            await _uow.SaveChangesAsync();

            return _mapper.Map<DoctorScheduleDto>(scheduleResult.Value);

             

        }



    }





}
